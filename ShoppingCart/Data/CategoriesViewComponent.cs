using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class CategoriesViewComponent:ViewComponent
    {
        private readonly AppDbContext db;

        public CategoriesViewComponent(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = await GetCategoriesAsync();
            return View(category);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return db.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
