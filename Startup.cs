using ElectronicStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace ElectronicStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
          

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(connection);
                options.EnableDetailedErrors();
            }
             );

            services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(Configuration["ConnectionStringsIdentity:DefaultConnection"]));

            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStringsOrder:DefaultConnection"]);
                options.EnableDetailedErrors();
            }
         );

            services.AddDbContext<CartContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStringsCart:DefaultConnection"]);
                options.EnableDetailedErrors();
            }
          );


            services.AddIdentity<User,IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 8;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
                opts.User.RequireUniqueEmail = true;    // уникальный email
            })
       .AddEntityFrameworkStores<AppIdentityDbContext>()
       .AddDefaultTokenProviders();

            
            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseStatusCodePagesWithRedirects("/404");
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();    
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
           
        }
    }
}
