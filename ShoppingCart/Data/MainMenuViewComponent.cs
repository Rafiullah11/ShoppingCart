using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class MainMenuViewComponent: ViewComponent
    {
        private readonly AppDbContext _db;

        public MainMenuViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pege = await GetPagesAsync();
            return View(pege);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return _db.Pages.OrderBy(x => x.Sorting).ToListAsync(); 
        }
    }
}
