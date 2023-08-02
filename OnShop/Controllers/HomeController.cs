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
       
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger, OnShopDBContext dbContext, UserManager<ApplicationUser> userManager) 
            : base(dbContext, userManager)
        {
            _logger = logger;
     
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

         
        public IActionResult Shoes()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 1).ToList();

            return View();
        }
        public IActionResult Tshirts()
        {

            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 2).ToList();

            return View();
        }

        
        
        public IActionResult Monitors()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 3).ToList();

            return View();
        }
       
        public IActionResult Keyboards()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 4).ToList();

            return View();
        }


        public IActionResult Mouse()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 5).ToList();

            return View();
        }

        public IActionResult Computer()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 3 || x.CategoryId == 4 || x.CategoryId == 5).ToList();

            return View();
        }
        public IActionResult ClothingShoes()
        {


            PopulateCartProductData();


            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 1 || x.CategoryId == 2).ToList();

            return View();
        }
        public IActionResult HomeGarden()
        {
            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 6).ToList();
            return View();
        }
        public IActionResult Accessories()
        {

            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 7).ToList();
            return View();
        }
        public IActionResult Tools()
        {

            ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 8).ToList();
            return View();
        }
      




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}