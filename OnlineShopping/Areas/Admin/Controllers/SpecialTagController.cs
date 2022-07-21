using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }
        [HttpGet]
        public IActionResult Create ()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Add(specialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product Type has been saved successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(specialTag);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _db.Update(specialTag);
                await _db.SaveChangesAsync();
                TempData["edit"] = "Product Type has been updated successfully";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(specialTag);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details (SpecialTag specialTag)
        {
            return RedirectToAction(actionName: nameof(Index));
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTag specialTag)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != specialTag.Id)
            {
                return NotFound();
            }
            var tag = _db.SpecialTags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(tag);
                await _db.SaveChangesAsync();
                TempData["delete"] = "Product Type has been deleted";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(tag);
        }
    }
}
