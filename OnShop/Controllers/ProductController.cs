using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
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
  
        private readonly ILogger<HomeController> _logger;


        public ProductController(OnShopDBContext dbContext, UserManager<ApplicationUser> userManager)
            : base(dbContext, userManager)
        {
              
             
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
            PopulateCartProductData();
            PopulateUserProductData();
            var userProducts = GetUserProducts();

          

            return View(userProducts);
        }


        protected void PopulateUserProductData()
        {
            List<UserProducts> userProducts = GetUserProducts();

            var userProductIds = new List<int>();
            var userProductNames = new List<string>();
            var userProductQuantities = new List<int>();
            var userProductPrices = new List<decimal>();
          

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
                    var productFromProductsTable = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);

                    if (productFromProductsTable != null)
                    {
                        _dbContext.Products.Remove(productFromProductsTable);
                        _dbContext.SaveChanges();
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

        [HttpGet]
        public IActionResult ProductSearch(string searchTerm)
        {

            PopulateCartProductData();
            var products = _dbContext.Products
                .Where(p => p.ProductName.Contains(searchTerm) ||
                            p.CategoryName.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm))
                .ToList();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Products = products;  

            return View();
        }
        [HttpGet]
        public IActionResult AddProduct()
        {

            PopulateCartProductData();
            var categories = _dbContext.Categories
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
                var categoryName = _dbContext.Categories
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
                
                var userProduct = new UserProducts
                {
                    
                    ProductName = viewModel.ProductName, 
                    
                    Price = viewModel.Price,
                    Quantity = viewModel.Quantity,
                    Description = viewModel.Description,
                    CategoryId = viewModel.CategoryId,
                    CategoryName = categoryName,
                    ImageUrl = viewModel.ImageUrl,
                    ApplicationUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId)
                };
               
                _dbContext.UserProducts.Add(userProduct);
                _dbContext.SaveChanges();
                 
                product.UserProductId = userProduct.UserProductId;
               
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                userProduct.ProductId = product.ProductId; 
                _dbContext.SaveChanges();

                return RedirectToAction("YourProducts");
            } 
            var categories = _dbContext.Categories
                .Select(p => new SelectListItem { Value = p.CategoryId.ToString(), Text = p.CategoryName })
                .ToList();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpGet] 
        public IActionResult EditProduct(int ProductId)
        {
            PopulateCartProductData();
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result; 
            var productToEdit = _dbContext.UserProducts.FirstOrDefault(p => p.ProductId == ProductId);

            productToEdit.ApplicationUser = user;

            if (productToEdit == null)
            {
                return NotFound();
            } 
            return View("EditProduct", productToEdit);
        } 
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct([Bind("UserProductId, ProductName,ProductId, Price, Description, Quantity, CategoryName, CategoryId, ImageUrl,ApplicationUser")] UserProducts productToEdit)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            productToEdit.ApplicationUser = user;
            var userProduct = _dbContext.UserProducts.FirstOrDefault(up => up.UserProductId == productToEdit.UserProductId && up.UserId == userId);
            var product = _dbContext.Products.FirstOrDefault(up => up.UserProductId == productToEdit.UserProductId && up.UserId == userId);

            if (ModelState.IsValid)
            { 
                if (userProduct != null)
                { 
                   userProduct.ProductId = product.ProductId;
                    product.ProductName = userProduct.ProductName = productToEdit.ProductName;
                    product.Price = userProduct.Price = productToEdit.Price;
                    product.Description = userProduct.Description = productToEdit.Description;
                    product.Quantity = userProduct.Quantity = productToEdit.Quantity;
                    product.CategoryName = userProduct.CategoryName = productToEdit.CategoryName;
                    product.ImageUrl = userProduct.ImageUrl = productToEdit.ImageUrl;
                    product.UserProductId = productToEdit.UserProductId;

                    UpdateCart( ); 
                    _dbContext.SaveChanges();
                } 
                return RedirectToAction("YourProducts");
            } 
            return View(productToEdit);
        } 
    }
}
