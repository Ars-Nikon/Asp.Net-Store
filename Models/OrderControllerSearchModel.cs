using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class OrderControllerSearchModel
    {
        public string Name { get; set; }
        public int? IdOrder { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        
    }
}
