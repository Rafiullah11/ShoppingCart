using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Areas.Admin.Controllers
{
   [Area("Admin")]
    public class PageController : Controller
    {
        private readonly AppDbContext _db;

        public PageController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in _db.Pages orderby p.Sorting select p;
            List<Page> list = await pages.ToListAsync();
            return View(list);
        }
        [HttpGet]
        //GetMethod/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var page = await _db.Pages.FirstOrDefaultAsync(page=>page.Id==id);
            if (page==null)
            {
                return NotFound();
            }
            return View(page);
        }

        [HttpGet]
        //GetMthod/Create
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        //Create/Post
        public async Task<IActionResult> Create(Page model)
        {
            if (ModelState.IsValid)
            {
               model.Slug = model.Title.ToLower().Replace(" ", "_");
               model.Sorting = 100;
                var slug = await _db.Pages.FirstOrDefaultAsync(x=>x.Slug==model.Slug);
                if (slug!=null)
                {
                    ModelState.AddModelError("", "the titel is already exist");
                    return View(model);
                }
               await _db.Pages.AddAsync(model);
               await _db.SaveChangesAsync();
                TempData["Success"] = "Record added successfullly..";
               return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        //Edit/Get/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _db.Pages.FirstOrDefaultAsync(page=>page.Id==id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        
        [HttpPost]
        //Create/Post
        public async Task<IActionResult> Edit(Page model)
        {
            if (ModelState.IsValid)
            {
                model.Slug = model.Id == 1 ? "home" : model.Title.ToLower().Replace(" ", "_");
                var slug = await _db.Pages.Where(x=>x.Id!=model.Id).FirstOrDefaultAsync(x => x.Slug == model.Slug);
                _db.Update(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Record Updated successfullly..";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        [HttpGet]
        //Delete/Get/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _db.Pages.FirstOrDefaultAsync(page=>page.Id==id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        
        [HttpPost]
        //Delete/Post
        public async Task<IActionResult> Delete(Page model)
        {
            if (model!=null)
            {
                _db.Remove(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Record Deleted successfullly..";
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}
