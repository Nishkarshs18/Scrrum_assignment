using Microsoft.AspNetCore.Mvc;
using Scrrum_Assignment.Data;
using Scrrum_Assignment.Models;
using System.Collections.Generic;
using System.Linq;

namespace Scrrum_Assignment.Controllers
{
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ChartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Retrieve all products from the database
            List<Product> products = _db.Products.ToList();
            return View(products);
        }

        public JsonResult GetChartData(int minPrice, int maxPrice)
        {
            // Retrieve filtered products based on the price range
            var filteredProducts = _db.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

            // Prepare data for the column chart
            var chartData = filteredProducts.Select(p => new { name = p.ProductTitle, y = p.Stock }).ToList();

            return Json(chartData);
        }
    }
}
