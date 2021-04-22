using ElectronicStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElectronicStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private UserManager<User> _userManager;
        private OrderContext orderContext;
        private CartContext cartContext;
        public AdminController(UserManager<User> userManager, OrderContext order, CartContext cart)
        {
            orderContext = order;
            cartContext = cart;
            _userManager = userManager;
        }


        public IActionResult Index(string Search, int pageSize = 16, int page = 1)
        {
            if (pageSize != 16 && pageSize != 32)
            {
                pageSize = 16;
            }


            if (Search != null)
            {
                var pagein = new PagingInfo { CurrentPage = page, pageSize = pageSize, ItemsPerPage = pageSize, TotalItem = 1 };
                List<User> Userlist = new List<User>();
                var User = _userManager.Users.FirstOrDefault(x => x.Email.ToLower() == Search.Trim().ToLower());
                if (User != null)
                {
                    Userlist.Add(User);
                    return View(new AdminIndexModel { PagingInfo = pagein, Users = Userlist });
                }
                User = _userManager.Users.FirstOrDefault(x => x.Name.ToLower() == Search.Trim().ToLower());
                if (User != null)
                {
                    Userlist.Add(User);
                    return View(new AdminIndexModel { PagingInfo = pagein, Users = Userlist });
                }
                return View(new AdminIndexModel { PagingInfo = pagein });
            }


            int countpage = _userManager.Users.Count();
            var pageinf = new PagingInfo { CurrentPage = page, pageSize = pageSize, ItemsPerPage = pageSize, TotalItem = countpage };


            List<User> Users = _userManager.Users.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.Date).ToList();


            return View(new AdminIndexModel { PagingInfo = pageinf, Users = Users });
        }

        public IActionResult Create() => View();





        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {


            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id, string Url)
        {
            if (Url != null)
            {
                Url = Url.Replace("//", "/");
                ViewBag.UrlBack = Url;
            }
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email, Name = user.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model, string Url)
        {

            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email.Trim();
                    user.UserName = model.UserName.Trim();
                    user.Name = model.Name.Trim();
                    var _passwordValidator =
                HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    if (model.Password != null)
                    {
                        IdentityResult resultPassword =
                await _passwordValidator.ValidateAsync(_userManager, user, model.Password.Trim());

                        if (resultPassword.Succeeded)
                        {
                            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password.Trim());
                        }
                        else
                        {
                            foreach (var error in resultPassword.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded && ModelState.IsValid)
                    {
                        if (Url != null)
                        {
                            Url = Url.Replace("//", "/");
                            return Redirect(Url);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id, string Url)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.UserName == "Admin")
                {
                    return RedirectToAction("Index");
                }
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    if (Url != null)
                    {
                        Url = Url.Replace("//", "/");
                        return Redirect(Url);
                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult OrdersController(OrderControllerModel orderController, int pageSize = 16, int page = 1)
        {
            if (pageSize != 16 && pageSize != 32)
            {
                pageSize = 16;
            }
            IQueryable<OrderList> orders = orderContext.OrderList.Include(x => x.Orders);
            if (orderController.orderSearch != null)
            {
                if (orderController.orderSearch.Name != null)
                {
                    orders = orders.Where(x => x.UserName.ToLower() == orderController.orderSearch.Name.ToLower().Trim());
                }
                if (orderController.orderSearch.IdOrder != null)
                {
                    orders = orders.Where(x => x.Id == orderController.orderSearch.IdOrder);
                }
                if (orderController.orderSearch.Date != null)
                {
                    orders = orders.Where(x => x.Date.Date == orderController.orderSearch.Date);
                }
                if (orderController.orderSearch.Status != null)
                {
                    if (orderController.orderSearch.Status != "ALL")
                    {
                        orders = orders.Where(x => x.Status == orderController.orderSearch.Status);
                    }

                }
            }


            int countpage = orders.Count();
            var pageinf = new PagingInfo { CurrentPage = page, pageSize = pageSize, ItemsPerPage = pageSize, TotalItem = countpage };

            if (orders == null)
            {
                return View(new OrderControllerModel { pagingInfo = pageinf });
            }

            List<OrderList> orderLists = orders.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.Date).ToList();
            return View(new OrderControllerModel { pagingInfo = pageinf, orderLists = orderLists });
        }

        [HttpPost]
        public ActionResult EditStatus(Status status, int Id_Order, string Url)
        {
            var order = orderContext.OrderList.FirstOrDefault(x => x.Id == Id_Order);
            if (order == null)
            {
                return RedirectToAction("OrdersController");
            }
            order.Status = status.ToString();
            orderContext.SaveChanges();
            if (Url != null)
            {
                Url = Url.Replace("//", "/");
                return Redirect(Url);
            }
            return RedirectToAction("OrdersController");
        }
    }

}
