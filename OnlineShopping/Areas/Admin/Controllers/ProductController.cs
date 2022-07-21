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
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;

                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been saved succefully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

    }
}
