using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    public class PageController : Controller
    {
        private readonly AppDbContext _db;
        public PageController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Pages(string slug)
        {
            if (slug ==null)
            {
                return View(await _db.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }

            var page = await _db.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if (page==null)
            {
                return NotFound();
            }
            //else
            //{
            //    return View(page);

            //}
            return View(page);
        }
    }
}
