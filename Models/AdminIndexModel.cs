using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class AdminIndexModel
    {
        public List<User> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
