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
            : base(dbContext, userManager)
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
            PopulateCartProductDataInViewBag();

            return View( );
        }
        public List<ShoppingCart> GetCartProducts()
        {

            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {

                return new List<ShoppingCart>();
            }


            var productsInCart = _dbContext.ShoppingCarts
                .Where(cart => cart.ApplicationUser.Id == userId)
                .ToList();

            return productsInCart;
        }

        [Authorize]
        public IActionResult Shoes()
        {

         
            PopulateCartProductDataInViewBag();

        
            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 1).ToList();

            return View();
        }

        public IActionResult test()
        {

            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 1).ToList();

            return View();
        }
        [Authorize]
        public IActionResult Tshirts()
        {
           
            PopulateCartProductDataInViewBag();

          
            ViewBag.Products = _dbContextProduct.Products.Where(x => x.CategoryId == 2).ToList();

            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private void PopulateCartProductDataInViewBag()
        {
            
            List<ShoppingCart> cartProducts = GetCartProducts();

       
            List<string> cartProductIds = new List<string>();
            List<string> cartProductNames = new List<string>();
            List<int> cartProductQuantities = new List<int>();
            List<decimal> cartProductPrices = new List<decimal>();
            decimal TotalPrice = 0;

            
            foreach (var cartProduct in cartProducts)
            {
                TotalPrice += cartProduct.Quantity * cartProduct.Price;
                cartProductIds.Add(cartProduct.ProductId);
                cartProductNames.Add(cartProduct.ProductName);
                cartProductQuantities.Add(cartProduct.Quantity);
                cartProductPrices.Add(cartProduct.Price);
            }
 
            ViewBag.CartProductIds = cartProductIds;
            ViewBag.CartProductNames = cartProductNames;
            ViewBag.CartProductQuantities = cartProductQuantities;
            ViewBag.CartProductPrices = cartProductPrices;

            ViewBag.TotalPrice = TotalPrice;
        }
    }
}