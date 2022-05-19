using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }
    }
}
