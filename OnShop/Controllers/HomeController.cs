using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnShop.Areas.Identity.Data;
using OnShop.Data;
using OnShop.Models;
using System.Diagnostics; 

namespace OnShop.Controllers
{
   public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;  //
        private readonly OnShopDBContext _dbContext;               //
        private readonly OnShopContext _dbContextProduct;      
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger, OnShopDBContext dbContext, OnShopContext dbContextProduct, UserManager<ApplicationUser> userManager) 
            : base(dbContext, dbContextProduct, userManager)
        {
            _logger = logger;
            _dbContextProduct = dbContextProduct;
            _userManager = userManager;
            _dbContext = dbContext;
        }



        [Authorize]
        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            PopulateCartProductData();

            return View();
        }

        [Authorize]
        public IActionResult Shoes()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 1).ToList();

            return View();
        }
        [Authorize]
        public IActionResult Monitor()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 3).ToList();

            return View();
        }
        [Authorize]
        public IActionResult Keyboard()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 4).ToList();

            return View();
        }


        [Authorize]
        public IActionResult Tshirts()
        {

            PopulateCartProductData();


            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 2).ToList();

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}