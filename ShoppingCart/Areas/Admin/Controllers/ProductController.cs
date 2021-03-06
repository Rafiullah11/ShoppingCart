using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index(int p=1)
        {
            int pageSize = 6;
            var product =  _context.Products.OrderByDescending(x => x.Id)
                                            .Include(x => x.Category)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            return View(await product.ToListAsync());
            //var appDbContext = _context.Products.Include(p => p.Category);
            //return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
           // ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }

        // POST: Admin/Product/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Slug,Description,Price,CategoryId,Image")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var slug = await _context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug!=null)
                {
                    ModelState.AddModelError("", "Product already exist..");
                    return View(product);
                }
                string imageName = "noimage.png";
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(hostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                   string filePath =Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }
                product.Image = imageName;

                await _context.AddAsync(product);
               await _context.SaveChangesAsync();
                TempData["Success"] = "Record added succefully..";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);

            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        //POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var slug = await _context.Products.Where(x=>x.Id!=id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Product already exist..");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(hostEnvironment.WebRootPath, "media/products");
                    if (!string.Equals(product.Image,"noimage.png"))
                    {
                        string oldImage = Path.Combine(uploadDir,product.Image);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;

                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Record Updated succefully..";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product==null)
            {
                return NotFound();
            }
            else
            {
                string uploadDir = Path.Combine(hostEnvironment.WebRootPath, "media/products");
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string oldImage = Path.Combine(uploadDir, product.Image);
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Record Deleted succefully..";

                return RedirectToAction(nameof(Index));
            }
        
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
