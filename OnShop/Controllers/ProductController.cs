using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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


        public ProductController(OnShopDBContext dbContext, OnShopContext dbContextProduct, UserManager<ApplicationUser> userManager)
            : base(dbContext, dbContextProduct, userManager)
        {

            _dbContextProduct = dbContextProduct;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [Authorize]

        [HttpGet]
        public IActionResult Profile()
        {
            PopulateCartProductData();

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
         
        [Authorize]
        [HttpGet]
        public IActionResult YourProducts()
        {
            PopulateUserProductData();
            var userProducts = GetUserProducts();

            
          
            return View(userProducts);
        }


        protected void PopulateUserProductData()
        {
            List<UserProducts> userProducts = GetUserProducts();

            List<int> userProductIds = new List<int>();
            List<string> userProductNames = new List<string>();
            List<int> userProductQuantities = new List<int>();
            List<decimal> userProductPrices = new List<decimal>();
          

            foreach (var userProduct in userProducts)
            {
               
                userProductIds.Add(userProduct.ProductId);
                userProductNames.Add(userProduct.ProductName);
                userProductQuantities.Add(userProduct.Quantity);
                userProductPrices.Add(userProduct.Price);
            }

            ViewBag.UserProductIds = userProductIds;
            ViewBag.UserProductNames = userProductNames;
            ViewBag.UserProductQuantities = userProductQuantities;
            ViewBag.UserProductPrices = userProductPrices;
           
        }
        public List<UserProducts> GetUserProducts()
        {

            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user == null)
            {
                return new List<UserProducts>();
            }

            var userProducts = _dbContext.UserProducts
                .Where(user => user.UserId == userId)
                .ToList();

            return userProducts;
 
        }


        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = _dbContextProduct.Categories
                .Select(p => new SelectListItem { Value = p.CategoryId.ToString(), Text = p.CategoryName })
                .ToList();

            ViewBag.Categories = categories;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProduct(AddProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);




              
                var categoryName = _dbContextProduct.Categories
                    .Where(c => c.CategoryId == viewModel.CategoryId)
                    .Select(c => c.CategoryName)
                    .FirstOrDefault();

          
                var product = new Products
                {
                    UserId = userId,
                    ProductName = viewModel.ProductName,
                    Price = viewModel.Price,
                    Quantity = viewModel.Quantity,
                    Description = viewModel.Description,
                    CategoryId = viewModel.CategoryId,
                    CategoryName = categoryName,
                    ImageUrl = viewModel.ImageUrl
                };
                _dbContextProduct.Products.Add(product);
                _dbContextProduct.SaveChanges();

              

             
                var userProduct = new UserProducts
                {
                    ProductId = product.ProductId,
                    UserId = userId,
                    ProductName = viewModel.ProductName,
                    Price = viewModel.Price,
                    Quantity = viewModel.Quantity,
                    Description = viewModel.Description,
                    CategoryId = viewModel.CategoryId,
                    CategoryName = categoryName,
                    ImageUrl = viewModel.ImageUrl
                };
                _dbContext.UserProducts.Add(userProduct);
                _dbContext.SaveChanges();

                return RedirectToAction("YourProducts");
            }

            var categories = _dbContextProduct.Categories
                .Select(p => new SelectListItem { Value = p.CategoryId.ToString(), Text = p.CategoryName })
                .ToList();

            ViewBag.Categories = categories;

            return View();
        }

 
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
          
        public IActionResult RemoveProduct(int productId)
        {


            PopulateUserProductData();
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            if (user != null && user.UserProducts != null)
            {
              
                var productToRemove = _dbContext.UserProducts.FirstOrDefault(item => item.ProductId == productId);

                if (productToRemove != null)
                {
                     
                    _dbContext.UserProducts.Remove(productToRemove);
                    _dbContext.SaveChanges();

                    
                    var productFromProductsTable = _dbContextProduct.Products.FirstOrDefault(p => p.ProductId == productId);
                    if (productFromProductsTable != null)
                    {
                        _dbContextProduct.Products.Remove(productFromProductsTable);
                        _dbContextProduct.SaveChanges();
                    }
                }
                else
                {
                  
                    return NotFound();
                }
            }

            string referringUrl = Request.Headers["Referer"].ToString();
            return Redirect(referringUrl);
        }







    }
}
