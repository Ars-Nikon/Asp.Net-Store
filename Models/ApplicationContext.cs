using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{

    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {

        }

        public DbSet<Phone> Phones { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

    }

}
