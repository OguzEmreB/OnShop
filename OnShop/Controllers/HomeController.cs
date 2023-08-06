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
        private readonly UserManager<ApplicationUser> _userManager; 
        private readonly OnShopDBContext _dbContext;   
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
            //ViewData["UserID"] = _userManager.GetUserId(this.User);
            PopulateCartProductData();

            return View();
        }

        public IActionResult Products(string category)
        {

            PopulateCartProductData();

            if (category == "Shoes")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 1).ToList();
               
            }
            else if (category == "Tshirts")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 2).ToList();
           
            }
            else if (category == "Monitors")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 3).ToList();
            }
            else if (category == "Keyboards")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 4).ToList();
            }
            else if (category == "Mouse")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 5).ToList();
            }
            else if (category == "Chairs")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 6).ToList();
            }
            else if (category == "Sofas")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 7).ToList();
            }
            else if (category == "WhiteGoods")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 8).ToList();
            }
            else if (category == "Pliers")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 9).ToList();
            }
            else if (category == "Screwdrivers")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 10).ToList();
            }
            else if (category == "Drills")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 11).ToList();
            }
            else if (category == "Hairbands")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 12).ToList();
            }
            else if (category == "Sunglasses")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 13).ToList();
            }
            else if (category == "Wallets")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 14).ToList();
            }
            else if (category == "Computer")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 3 || x.CategoryId == 4 || x.CategoryId == 5).ToList();
            }
            else if (category == "Clothing-Shoes")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 1 || x.CategoryId == 2).ToList();
            }
            else if (category == "Home-Garden")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 6 || x.CategoryId == 7 || x.CategoryId == 8).ToList();
            }
            else if (category == "Accessories")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 12 || x.CategoryId == 13 || x.CategoryId == 14).ToList();
            }
            else if (category == "Tools")
            {
                ViewBag.Products = _dbContext.Products.Where(x => x.CategoryId == 9 || x.CategoryId == 10 || x.CategoryId == 11).ToList();
            }

            ViewBag.Title = category;
            return View();
        }
      
   
       
    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}