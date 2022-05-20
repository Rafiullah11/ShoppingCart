using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Data;
using System;
using System.Linq;

namespace ShoppingCart.Models
{
    public class SaeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }
                context.Pages.AddRange(
               new Page
               {     
                    Title="Home",       
                    Slug="home",       
                    Content="home page",       
                    Sorting=0       
               },
               new Page
               {     
                    Title="About Us",       
                    Slug="about-us",       
                    Content="about us page",       
                    Sorting=100       
               },
               new Page
               {     
                    Title="Services",       
                    Slug="service",       
                    Content= "service page",       
                    Sorting=100       
               },
               new Page
               {     
                    Title="Contact",       
                    Slug= "contact",       
                    Content= "contact page",       
                    Sorting=100       
               }
               );
                context.SaveChanges();
            }
        }
    }
}
