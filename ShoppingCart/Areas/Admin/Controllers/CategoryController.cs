using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            return View(await _db.Categories.OrderBy(x=>x.Sorting).ToListAsync());
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)

        {
            var get = await _db.Categories.FindAsync(id);
            if (get == null)
            {
                return NotFound();
            }
            return View(get);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Slug = model.Name.ToLower().Replace(" ", "_") ;
                    model.Sorting = 100;
                    var slug = await _db.Categories.FirstOrDefaultAsync(x=>x.Slug==model.Slug);
                    if (slug!=null)
                    {
                        ModelState.AddModelError("", "Category already exist");
                        return View (model);
                    }
                    await _db.AddAsync(model);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Record added successfullly..";
                    return RedirectToAction(nameof(Index));


                }
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var get =await _db.Categories.FindAsync(id);
            if (get == null) 
            {
                return NotFound();
            }
            return View(get);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category model, int id)
        {
           
                if (ModelState.IsValid)
                {
                    model.Slug = model.Name.ToLower().Replace(" ", "_");
                   var slug= await _db.Categories.Where(x=>x.Id!=id).FirstOrDefaultAsync(x=>x.Slug==model.Slug);
                    if (slug!=null)
                    {
                        ModelState.AddModelError("", "Category already exists.. ");
                        return View(model);
                    }
                    _db.Update(model);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Record Updated successfullly..";

                    return RedirectToAction(nameof(Index));

                }
                return View(model);
            
           
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var get = await _db.Categories.FindAsync(id);
            if (get == null)
            {
                return NotFound();
            }
            return View(get);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Category model)
        {
            if (model != null)
            {
                _db.Remove(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Record Deleted successfullly..";
                return RedirectToAction("Index");
            }
            return View(model);

        }

        [HttpPost]
        // Post Admin/Page/Reorder
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            foreach (var pageId in id)
            {
                var page = await _db.Categories.FindAsync(pageId);
                page.Sorting = count;
                _db.Update(page);
                await _db.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
