using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class OrderViewModel
    {
        public IEnumerable<Phone> phones { get; set; }
        public Dictionary<string, int> Order { get; set; }
    }
}
