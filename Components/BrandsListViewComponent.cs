using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicStore.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace ElectronicStore.Components
{
    public class BrandsListViewComponent : ViewComponent
    {
        private List<Brand> Brands;

        private ApplicationContext db;

        public BrandsListViewComponent(ApplicationContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke(PhonesAndCatalog phonesAndCatalog)
        {

            ViewBag.MaxPrice = phonesAndCatalog.MaxPriceForBrandList;
            ViewBag.MinPrice = phonesAndCatalog.MinPriceForBrandList;

            Dictionary<string, int> ProductAndCount = new Dictionary<string, int>();
            Brands = db.Brands.ToList();
            foreach (var item in Brands)
            {
                ProductAndCount.Add(item.Name,db.Phones.Where(x=>x.Id_Brands == item.Id).Count());
            }
           
            return View("BrandsList", new Catalog { Sorting = phonesAndCatalog.Sorting, ProductAndCount = ProductAndCount, pageSize = phonesAndCatalog.PagingInfo.pageSize});
        }

    }
}
