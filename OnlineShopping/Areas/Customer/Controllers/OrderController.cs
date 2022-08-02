using Microsoft.AspNetCore.Mvc;
using OnlineShopping.Data;
using OnlineShopping.Models;
using OnlineShopping.Utility;

namespace OnlineShopping.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.ProductId = product.Id;             
                    anOrder.OrderDetails.Add(orderDetail);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", null);
            return View();
        }

        public string GetOrderNo()
        {
            int rowCount=_db.Orders.ToList().Count+1;
            return rowCount.ToString("000");
        }
    }
}

