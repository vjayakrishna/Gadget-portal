using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineGadgetStore.Data;
using OnlineGadgetStore.Models;

namespace OnlineGadgetStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.Include(p=>p.ImageURLs).ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from p in _context.Product.Include(p => p.ImageURLs)
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }
            TempData["Name"] = searchString;
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p=>p.ImageURLs).Include(p=>p.ProductInfo)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,Type,Description,ProductInfo")] Product product, String filenames)
        {
            if (ModelState.IsValid)
            {
                product.ImageURLs = new List<Image>();
                if(filenames !=null)
                {
                   var fnames= filenames.Split('|').Skip(1);
                    foreach(var name in fnames)
                    {
                        var img = new Image
                        {
                            URL = name,
                            Product = product
                        };
                        product.ImageURLs.Add(img);
                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //POST:Products/Create
        [HttpPost]
        public async Task<ActionResult> UploadImages(List<IFormFile> pictures)
        {
            var newFileName = "";
            var imagePath = "";
            Image img=null;

            if (pictures != null)
            {
                foreach(var pic in pictures)
                {
                    newFileName = //Guid.NewGuid().ToString() + "_" +
                            Path.GetFileName(pic.FileName);
                    imagePath = @"wwwroot\images\Products\" + newFileName;
                    TempData["FileName"] += "|"+newFileName;
                    await pic.CopyToAsync(new FileStream(imagePath, FileMode.Create));
                }
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p=>p.ImageURLs).Include(p=>p.ProductInfo).SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,Type,Description,ProductInfo")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //POST:Products/Edit
        [HttpPost]
        public async Task<IActionResult> EditImages(int id, List<IFormFile> pictures)
        {
            var newFileName = "";
            var imagePath = "";
            Image img = null;

            if (pictures != null)
            {
                foreach (var pic in pictures)
                {
                    newFileName = //Guid.NewGuid().ToString() + "_" +
                            Path.GetFileName(pic.FileName);
                    imagePath = @"wwwroot\images\Products\" + newFileName;
                    TempData["FileName"] += "|" + newFileName;
                    await pic.CopyToAsync(new FileStream(imagePath, FileMode.Create));
                    img = new Image
                    {
                        URL = newFileName,
                        ProductID = id
                    };
                    _context.Add(img);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Edit",new { id });
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p=>p.ImageURLs).Include(p=>p.ProductInfo)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.Include(p=>p.ImageURLs).Include(p=>p.ProductInfo).SingleOrDefaultAsync(m => m.ID == id);
            var filenames = product.ImageURLs.Select(i=>i.URL).ToList();
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            foreach (var fname in filenames)
            {
                if (System.IO.File.Exists(@"wwwroot\images\Products\" + fname))
                {
                    try
                    {
                        System.IO.File.Delete(@"wwwroot\images\Products\" + fname);
                    }
                    catch(IOException)
                    {
                        continue;
                    }
                }
            }
            //new FileStream(@"wwwroot\images\Products\",FileAccess.ReadWrite).FlushAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
