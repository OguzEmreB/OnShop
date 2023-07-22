using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnShop.Areas.Identity.Data;
using OnShop.Models;
using System.Diagnostics;

namespace OnShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OnShopContext _context;
        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager, OnShopContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
            
        }

        [Authorize]
        public IActionResult Index()
        {

           ViewData["UserID"]= _userManager.GetUserId(this.User);
            return View();
        }

        [Authorize]
        public IActionResult Ayakkabilar()
        {

            ViewBag.Products = _context.Products.Where(x => x.CategoryId == 1).ToList();

            return View();
        }
        [Authorize]
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