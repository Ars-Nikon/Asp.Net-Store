using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class OrderControllerModel
    {
        public OrderControllerSearchModel orderSearch { get; set; }
        public PagingInfo pagingInfo { get; set; }
        public List<OrderList> orderLists { get; set; }
        public Status Status { get; set; }
    }
}
