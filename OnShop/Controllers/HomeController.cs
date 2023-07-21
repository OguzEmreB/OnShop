using Microsoft.AspNetCore.Mvc;
using OnShop.Models;
using System.Diagnostics;

namespace OnShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnShopContext _context;
        public HomeController(ILogger<HomeController> logger, OnShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    
        public IActionResult Ayakkabilar()
        {

            ViewBag.Products = _context.Products.Where(x => x.CategoryId == 1).ToList();

            return View();
        }
        public IActionResult Tshirt()
        {
            ViewBag.Products = _context.Products.Where(x => x.CategoryId == 2).ToList();
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}