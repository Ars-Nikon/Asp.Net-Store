using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class Order
    {
        [ForeignKey("Id_OrderList")]
        public OrderList OrderList { get; set; }
        public int Id { get; set; }
        public int Id_Product { get; set; }
        public int Quantity { get; set; }
        public int Id_OrderList { get; set; }
        public string Name { get; set; }

    }

    public enum Status
    {
        Processing,
        Delivered,
        Canceled,
        Received
    }
    public class OrderList
    {
        public int Id { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
        public string UserName { get; set; }
        public string Id_User { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Time)]
        public DateTime Date 
        {
            get 
            {
                return _date.AddHours(3);
            }
            set
            {
                _date = value;
            } 
        }
        [NotMapped]
        private DateTime _date;
    }
}
