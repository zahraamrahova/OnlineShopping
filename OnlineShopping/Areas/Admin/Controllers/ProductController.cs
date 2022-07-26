using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _he;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(s => s.SpecialTag).ToList());
        }

        [HttpPost]
        public IActionResult Index (decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount==null || largeAmount==null)
            {
                 products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchproduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
             
                if (searchproduct!=null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                    return View(product);
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;

                }
                if (image==null)
                {
                    product.Image = "Images/noimage.png";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been saved succefully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [HttpGet]
        public ActionResult Edit (int? id) 
        {
            ViewData["ProductTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;

                }
                if (image == null)
                {
                    product.Image = "Images/noimage.png";
                }
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been saved succefully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);

        }
        [HttpGet]
        public ActionResult Details (int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpGet]
        public ActionResult Delete (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(d => d.Id == id);
            if (product==null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(d => d.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
           await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
