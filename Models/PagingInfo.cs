using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class PagingInfo
    {
        public int TotalItem { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int pageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItem / ItemsPerPage);
    }
}
