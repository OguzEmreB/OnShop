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
        protected readonly OnShopContext _dbContextProduct;
        protected readonly UserManager<ApplicationUser> _userManager;

        public BaseController(OnShopDBContext dbContext, OnShopContext dbContextProduct, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _dbContextProduct = dbContextProduct;
        }

    


        protected List<ShoppingCart> GetCartProducts()
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


            foreach (var product in productsInCart)
            {
                var isOutOfStock = !_dbContextProduct.Products.Any(p => p.ProductId == product.ProductId);
                product.StockStatus = isOutOfStock ? "Out of Stock" : "In Stock";
                if (isOutOfStock)
                {
              
                    product.Price = 0;
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

       


    }
}
