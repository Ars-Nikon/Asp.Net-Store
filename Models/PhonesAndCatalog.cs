using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{

    public class BrandCatalog
    {
       public string Name;
        public bool OnorOf; 
    }

    public class PhonesAndCatalog
    {
        public int? MinPrice;

        public int? MaxPrice;

        public int? MaxPriceForBrandList;

        public int? MinPriceForBrandList;

        public PagingInfo PagingInfo;

        public bool Sorting { get; set; }

        public Dictionary<string, bool> catalog;

        public IEnumerable<Phone> phones;
    }
}
