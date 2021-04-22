using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class CartContext : DbContext
    {
        public CartContext()
        {

        }

        public DbSet<Cart> Carts { get; set; }
      
        public CartContext(DbContextOptions<CartContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

    }
}

