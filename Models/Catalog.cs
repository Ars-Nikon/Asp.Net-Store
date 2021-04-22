using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class Catalog
    {
        public string Name { get; set; }

        public Dictionary<string, int> ProductAndCount { get; set; }

        public int? MinPrice { get; set; }

        public Dictionary<string, bool> catalog;

        public int pageSize { get; set; }

        public int? MaxPrice { get; set; }

       public bool Sorting { get; set; }

    }
}
