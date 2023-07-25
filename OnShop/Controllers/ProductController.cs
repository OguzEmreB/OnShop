using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnShop.Areas.Identity.Data;
using OnShop.Data;
using OnShop.Models;
using System.Linq;

namespace OnShop.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;  //
        private readonly OnShopDBContext _dbContext;                 //
        private readonly OnShopContext _dbContextProduct;
        private readonly ILogger<HomeController> _logger;


        public ProductController(ILogger<HomeController> logger, OnShopDBContext dbContext, OnShopContext dbContextProduct, UserManager<ApplicationUser> userManager)
            : base(dbContext, userManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [Authorize]

        [HttpGet]
        public IActionResult Profile()
        {
            PopulateCartProductDataInViewBag();

            var productsInCart = GetCartProducts();


            ViewBag.ProductsInCart = productsInCart;


            decimal totalPrice = 0;


            foreach (var product in productsInCart)
            {
                totalPrice += product.Price * product.Quantity;
            }


            ViewBag.TotalPrice = totalPrice;




            return View();
        }

        [HttpGet]
        public IActionResult YourProducts()
        {


            return View();
        }

        [HttpPost]
        public IActionResult AddToCart( )
        { 

            return RedirectToAction( );
        }

        [HttpPost]
        public IActionResult RemoveFromCart( )
        {
           

            return RedirectToAction( );
        }
    }
}
