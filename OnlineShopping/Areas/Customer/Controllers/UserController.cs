using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
             _userManager= userManager;
            _db= db;
        }
        public IActionResult Index()
        {
            var dd = _userManager.GetUserId(HttpContext.User);
            return View(_db.ApplicationUSers.ToList());

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                    TempData["save"] = "User has been created successfully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user =  _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user==null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                //var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                TempData["save"] = "User has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Lockout(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            
            userInfo.LockoutEnd=DateTime.Now.AddYears(100);
            int rowEffected= _db.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["delete"] = "User has been lockout successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Active(string id)
        {
            var user =_db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Active(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowEffected = _db.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["save"] = "User has been activated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.ApplicationUSers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Delete(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUSers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            _db.ApplicationUSers.Remove(userInfo);
            int rowEffected = _db.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["delete"] = "User has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
