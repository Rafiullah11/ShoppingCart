using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }
        //Get/product
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var product = _db.Products.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_db.Products.Count() / pageSize);

            return View(await product.ToListAsync());
        }
        //Get/ProductByCategory
        public async Task<IActionResult> ProductByCategory(string categorySlug, int p = 1)
        { 
            Category category =await _db.Categories.Where(x=>x.Slug==categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");

            
            int pageSize = 6;
            var product = _db.Products.OrderByDescending(x => x.Id)
                                            .Where(x=>x.CategoryId==category.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
                                            
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_db.Products.Where(x => x.CategoryId == category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;
            return View(await product.ToListAsync());
        }

    }
}
