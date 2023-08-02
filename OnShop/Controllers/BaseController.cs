using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnShop.Areas.Identity.Data;
using OnShop.Data;
using OnShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnShop.Controllers
{

 
    public class BaseController : Controller
    {
        protected readonly OnShopDBContext _dbContext; 
        protected readonly UserManager<ApplicationUser> _userManager;

        public BaseController(OnShopDBContext dbContext,UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
          
        }

    


        protected List<ShoppingCart> GetCartProducts()
        {
            UpdateCart();

            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users
            .Include(u => u.ShoppingCart) 
        .FirstOrDefault(u => u.Id == userId);



            if (user == null)
            {
                return new List<ShoppingCart>();
            }

            var productsInCart = _dbContext.ShoppingCarts
                .Where(cart => cart.ApplicationUser.Id == userId)
                .ToList();


            foreach (var product in productsInCart)
            {
                var isOutOfStock = !_dbContext.Products.Any(p => p.ProductId == product.ProductId);
                product.StockStatus = isOutOfStock ? "Out of Stock" : "In Stock";
                if (isOutOfStock)
                {
              
                    product.Price = 0;
                    product.Quantity = 0;
                }
            }
            return productsInCart;
        }

        protected void PopulateCartProductData()
        {
            List<ShoppingCart> cartProducts = GetCartProducts();

            List<int> cartProductIds = new List<int>();
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


        [Authorize]
        [HttpPost]
        public void UpdateCart()
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users
                .Include(u => u.ShoppingCart)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return;
            }

            var productsInCart = user.ShoppingCart.ToList();

            foreach (var cartProduct in productsInCart)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == cartProduct.ProductId);

                if (product != null)
                {
                    cartProduct.ProductName = product.ProductName;
                    cartProduct.Price = product.Price;
                    cartProduct.Description = product.Description;
                    cartProduct.ImageUrl = product.ImageUrl;
                    
 
                }
            }

            _dbContext.SaveChanges();
        }



    }
}
