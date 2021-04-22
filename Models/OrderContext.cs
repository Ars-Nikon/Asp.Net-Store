using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext()
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderList> OrderList { get; set; }
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
          
        }

    }
}
