using ElectronicStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace ElectronicStore.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> userManager;
        private ApplicationContext db;
        private CartContext CartContext;
        private OrderContext OrderContext;
        private readonly SignInManager<User> _signInManager;

        public HomeController(ApplicationContext applicationContext, OrderContext order, UserManager<User> usrMgr, SignInManager<User> signInManager, CartContext cart)
        {
            OrderContext = order;
            CartContext = cart;
            userManager = usrMgr;
            _signInManager = signInManager;
            db = applicationContext;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int? Id, string Url = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var phoneEdit = db.Phones.FirstOrDefault(x => x.Id == Id);

            if (phoneEdit == null)
            {
                return NotFound();
            }

            var Brands = db.Brands.ToList();
            List<string> BrandsNameList = new List<string>();
            foreach (var item in Brands)
            {
                BrandsNameList.Add(item.Name ?? "Null");
            }
            ViewBag.BrandsStrName = BrandsNameList;
            if (Url != null)
            {
                Url = Url.Replace("//", "/");
                ViewBag.UrlBack = Url;
            }
            return View(phoneEdit);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(Phone phone)
        {

            if (phone.Image != null)
            {
                if (phone.Image.Length > 5244880)
                {
                    ModelState.AddModelError("", "Large file size, max. 5 MB");
                }
            }
            if (phone.Image != null)
            {
                using (var binaryReader = new BinaryReader(phone.Image.OpenReadStream()))
                {
                    phone.Path_image = binaryReader.ReadBytes((int)phone.Image.Length);
                }
            }
            if (phone.URL_Downloud_Image != null)
            {
                using (WebClient client = new WebClient())
                {
                    phone.Path_image = client.DownloadData(phone.URL_Downloud_Image.Trim());
                }
            }
            if (phone.Brands != null)
            {
                phone.Id_Brands = db.Brands.FirstOrDefault(X => X.Name == phone.Brands.Trim()).Id;
            }
            db.Phones.Update(phone);
            db.SaveChanges();


            if (phone.UrlBack != null)
            {
                phone.UrlBack = phone.UrlBack.Replace("//", "/");
                return Redirect(phone.UrlBack);
            }
            return RedirectToAction("Phones");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int? Id, string Url = null)
        {

            if (Id != null)
            {
                var Product = db.Phones.FirstOrDefault(x => x.Id == Id);
                if (Product == null)
                {
                    return RedirectToAction("Phones");
                }
                db.Phones.Remove(Product);
                db.SaveChanges();
            }
            if (Url != null)
            {
                Url = Url.Replace("//", "/");
                return Redirect(Url);
            }
            return RedirectToAction("Phones");
        }


        [HttpGet]
        public IActionResult Phones(Dictionary<string, bool> catalog, int? MinPrice, int? MaxPrice, int page = 1, bool Sorting = false, int pageSize = 16)
        {

            if (pageSize != 16 && pageSize != 32)
            {
                pageSize = 16;
            }

            IQueryable<Phone> phones = db.Phones;

            if (catalog.Count > 0)
            {
                int numberOfEnabledButtons = 0;
                var idlidt = new List<int>();
                foreach (var item in catalog)
                {
                    if (item.Value)
                    {
                        var Brandid = db.Brands.FirstOrDefault(x => x.Name == item.Key);
                        if (Brandid == null)
                        {
                            continue;
                        }
                        idlidt.Add(Brandid.Id);




                    }
                    else
                    {
                        numberOfEnabledButtons++;
                    }
                }
                if (numberOfEnabledButtons == catalog.Count)
                {
                    phones = db.Phones;
                }
                else
                {
                    phones = db.Phones.Where(x => idlidt.Any(y => y == x.Id_Brands));
                }
            }
            else
            {
                phones = db.Phones;
            }



            if (MinPrice != null && MaxPrice != null)
            {
                if (MinPrice > MaxPrice)
                {
                    int? a = MaxPrice;
                    MaxPrice = MinPrice;
                    MinPrice = a;
                }

                phones = phones.Where(x => (x.Price >= MinPrice && x.Price <= MaxPrice));
            }
            else
            {
                if (MinPrice != null)
                {

                    phones = phones.Where(x => (x.Price >= MinPrice && x.Price <= db.Phones.Max(x => x.Price)));
                }
                if (MaxPrice != null)
                {

                    phones = phones.Where(x => (x.Price >= db.Phones.Min(x => x.Price) && x.Price <= MaxPrice));
                }
            }




            int? MaxPriceForBrandList;

            int? MinPriceForBrandList;
            var countpage = phones.Count();
            if (countpage > 0)
            {
                MaxPriceForBrandList = phones.Max(x => x.Price);

                MinPriceForBrandList = phones.Min(x => x.Price);
            }
            else
            {
                MaxPriceForBrandList = 0;

                MinPriceForBrandList = 0;
            }

            if (Sorting)
            {
                phones = phones.OrderBy(x => x.Price);
            }
            else
            {
                phones = phones.OrderByDescending(x => x.Price);
            }

            List<Phone> Products = phones.Skip((page - 1) * pageSize).Take(pageSize).Select(x=> new Phone { Price = x.Price, Id= x.Id, Path_image = x.Path_image,Name = x.Name}).ToList();

            return View(new PhonesAndCatalog { Sorting = Sorting, phones = Products, MaxPrice = MaxPrice, MinPrice = MinPrice, catalog = catalog, MinPriceForBrandList = MinPriceForBrandList, MaxPriceForBrandList = MaxPriceForBrandList, PagingInfo = new PagingInfo { CurrentPage = page, pageSize = pageSize, ItemsPerPage = pageSize, TotalItem = countpage } });
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = db.Phones.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProductInCart(int? Id, string Url)
        {

            if (Id == null)
            {
                return RedirectToAction("Phones");//error
            }
            var Product = db.Phones.FirstOrDefault(x => x.Id == Id);
            if (Product == null)
            {
                return RedirectToAction("Phones");//error
            }
            var userId = await userManager.FindByNameAsync(User.Identity.Name);
            if (userId == null)
            {
                return RedirectToAction("Phones");//error
            }
            Cart cart = new Cart() { Id_Products = Product.Id, Id_User = userId.Id };

            var result = CartContext.Carts.FirstOrDefault(x => (x.Id_Products == cart.Id_Products) && (x.Id_Products == cart.Id_Products));
            if (result != null)
            {
                if (Url == null)
                {
                    return RedirectToAction("Phones");//error
                }
                Url = Url.Replace("//", "/");
                return Redirect(Url);
            }

            CartContext.Carts.Add(cart);
            CartContext.SaveChanges();

            if (Url == null)
            {
                return RedirectToAction("Phones");//error
            }
            Url = Url.Replace("//", "/");
            return Redirect(Url);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create(string Url)
        {

            var Brands = db.Brands.ToList();
            List<string> BrandsNameList = new List<string>();
            foreach (var item in Brands)
            {
                BrandsNameList.Add(item.Name ?? "Null");
            }
            ViewBag.BrandsStrName = BrandsNameList;

            if (Url != null)
            {
                Url = Url.Replace("//", "/");
                ViewBag.UrlBack = Url;
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Create(Phone phone)
        {
            if (phone.Image != null)
            {
                if (phone.Image.Length > 5244880)
                {
                    ModelState.AddModelError("", "Large file size, max. 5 MB");
                }
            }

            if (!ModelState.IsValid)
            {
                var Brands = db.Brands.ToList();
                List<string> BrandsNameList = new List<string>();
                foreach (var item in Brands)
                {
                    BrandsNameList.Add(item.Name ?? "Null");
                }
                ViewBag.BrandsStrName = BrandsNameList;
                return View();
            }
            if (phone.Image != null)
            {
                using (var binaryReader = new BinaryReader(phone.Image.OpenReadStream()))
                {
                    phone.Path_image = binaryReader.ReadBytes((int)phone.Image.Length);
                }
            }

            var BrandsHave = db.Brands.FirstOrDefault(x => x.Name == phone.Brands.Trim());
            if (BrandsHave == null)
            {
                db.Brands.Add(new Brand { Name = phone.Brands.Trim(), Id_Catalogs = 1 });
                db.SaveChanges();
                phone.Id_Brands = db.Brands.FirstOrDefault(X => X.Name == phone.Brands.Trim()).Id;
            }
            else
            {
                phone.Id_Brands = BrandsHave.Id;
            }
            db.Add(phone);
            db.SaveChanges();
            return RedirectToAction("Phones");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteInCart(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Cart");
            }
            var UserId = await userManager.FindByNameAsync(User.Identity.Name);
            var Products = CartContext.Carts.FirstOrDefault(x => (x.Id_User == UserId.Id) && (x.Id_Products == Id));
            if (Products == null)
            {
                return RedirectToAction("Cart");
            }
            CartContext.Carts.Remove(Products);
            CartContext.SaveChanges();
            return RedirectToAction("Cart");
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var UserId = await userManager.FindByNameAsync(User.Identity.Name);

            var ProductsList = CartContext.Carts.Where(x => x.Id_User == UserId.Id);

            List<int> ProductsId = new List<int>();

            if (ProductsList == null)
            {
                return View();
            }

            foreach (var item in ProductsList)
            {
                ProductsId.Add(item.Id_Products);
            }
            var Phones = db.Phones.Where(x => (ProductsId.Any(y => y == x.Id))).ToList();
            if (Phones == null)
            {
                return View();
            }
            ViewBag.FinalPrice = Phones.Where(x => x.Quantity > 0).Sum(x => x.Price);
            return View(Phones);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Order()
        {
            var UserId = await userManager.FindByNameAsync(User.Identity.Name);

            var ProductsList = CartContext.Carts.Where(x => x.Id_User == UserId.Id);

            List<int> ProductsId = new List<int>();

            if (ProductsList == null)
            {
                return View();
            }

            foreach (var item in ProductsList)
            {
                ProductsId.Add(item.Id_Products);
            }


            var Phones = db.Phones.Where(x => (ProductsId.Any(y => y == x.Id) && (x.Quantity > 0))).ToList();

            if (Phones == null)
            {
                return View();
            }



            return View(new OrderViewModel { phones = Phones });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FinalOrder(Dictionary<string, int> Order)
        {
            Dictionary<int, int> ProductDictionary = new Dictionary<int, int>();
            foreach (var item in Order)
            {
                int key;
                if (int.TryParse(item.Key, out key))
                {
                    ProductDictionary.Add(key, item.Value);
                }
            }


            var UserId = await userManager.FindByNameAsync(User.Identity.Name);
            if (ProductDictionary == null)
            {
                RedirectToAction("Order");
            }

            List<Order> finallist = new List<Order>();
            foreach (var item in ProductDictionary)
            {
                var productCart = CartContext.Carts.FirstOrDefault(x => (x.Id_User == UserId.Id) && (x.Id_Products == item.Key));
                if (productCart == null)
                {
                    ModelState.AddModelError("", $"Product not found");
                    break;
                }
                var product = db.Phones.FirstOrDefault(x => x.Id == productCart.Id_Products);
                if (product == null)
                {
                    ModelState.AddModelError("", $"Product not found");
                    break;
                }
                if ((product.Quantity - item.Value) < 0)
                {
                    ModelState.AddModelError("", $"Quantity left: {product.Quantity}");
                    break;
                }
                product.Quantity = product.Quantity - item.Value;
                db.Phones.Update(product);
                CartContext.Carts.Remove(productCart);

                finallist.Add(new Order { Name = product.Name, Id_Product = product.Id, Quantity = item.Value });
            }
            if (!ModelState.IsValid)
            {
                var ProductsList = CartContext.Carts.Where(x => x.Id_User == UserId.Id);
                List<int> ProductsId = new List<int>();
                foreach (var item in ProductsList)
                {
                    ProductsId.Add(item.Id_Products);
                }
                var Phones = db.Phones.Where(x => (ProductsId.Any(y => y == x.Id) && (x.Quantity > 0))).ToList();
                return View("Order", new OrderViewModel { phones = Phones });
            }
           
            DateTime dateTime = DateTime.UtcNow;
            OrderContext.OrderList.Add(new OrderList { Id_User = UserId.Id, UserName = UserId.UserName, Date = dateTime ,Status = Status.Processing.ToString()});
            OrderContext.SaveChanges();

            var orderlist = OrderContext.OrderList.OrderBy(x => x.Date).Last(x => x.Id_User == UserId.Id);

            foreach (var item in finallist)
            {
                item.Id_OrderList = orderlist.Id;
            }
            OrderContext.Orders.AddRange(finallist);
            db.SaveChanges();
            CartContext.SaveChanges();
            OrderContext.SaveChanges();
            return RedirectToAction("MyOrder");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyOrder()
        {
            var UserId = await userManager.FindByNameAsync(User.Identity.Name);

            var orders = OrderContext.OrderList.Where(x => x.Id_User == UserId.Id).Include(x => x.Orders).ToList();

            if (orders == null)
            {
                return View();
            }
           
            return View(orders.OrderByDescending(x=>x.Date));
        }
    }
}
