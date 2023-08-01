
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnShop.Models;
using OnShop.Data;
using System.Collections.Generic;
using System.Linq;
using OnShop.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OnShop.Controllers;
using NuGet.Protocol.Plugins;
using System.Security.Principal;

public class ShoppingCartController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;  //
    private readonly OnShopDBContext _dbContext;               //
   
    


    public ShoppingCartController(OnShopDBContext dbContext,  UserManager<ApplicationUser> userManager)
        : base(dbContext, userManager)
    {
        
        
        _userManager = userManager;
        _dbContext = dbContext;
    }
   
  
    [Authorize]
    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
   PopulateCartProductData();

        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

        if (user != null && user.ShoppingCart != null)
        {
            var productToRemove = user.ShoppingCart.FirstOrDefault(item => item.ProductId == productId);

            if (productToRemove != null)
            {
                user.ShoppingCart.Remove(productToRemove);
                _userManager.UpdateAsync(user).Wait();
            }
        }

        string referringUrl = Request.Headers["Referer"].ToString();
        
        return Redirect(referringUrl);
    }
     
   
    [HttpPost]
  
    public IActionResult AddToCart(int productId, int quantity)
    {
        if (!User.Identity.IsAuthenticated)   
        {
            return LocalRedirect("~/Identity/Account/Login");

        }

        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

       
        if (user.ShoppingCart == null)
        {
            user.ShoppingCart = new List<ShoppingCart>();
        } 
   
        var productToAdd = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);

        
        if (productToAdd != null && !user.ShoppingCart.Any(item => item.ProductId == productToAdd.ProductId))
        { 
            var shoppingCartItem = new ShoppingCart
            {
                ProductId = productToAdd.ProductId,
                ProductName = productToAdd.ProductName, 
                Price=productToAdd.Price, 
                Quantity = quantity,
                ImageUrl = productToAdd.ImageUrl
            };
            user.ShoppingCart.Add(shoppingCartItem);
        } 
    
        _userManager.UpdateAsync(user).Wait();

       
        string referringUrl = Request.Headers["Referer"].ToString();
       
        return Redirect(referringUrl);
    }


    [Authorize]
    [HttpPost]

    public IActionResult ClearCart()
    {
        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

        if (user != null)
        {

            var userCartItems = _dbContext.ShoppingCarts.Where(cart => cart.ApplicationUser.Id == userId);
            _dbContext.ShoppingCarts.RemoveRange(userCartItems);
            _dbContext.SaveChanges();


            user.ShoppingCart.Clear();


            _userManager.UpdateAsync(user).Wait();
        }

        string referringUrl = Request.Headers["Referer"].ToString();

        return Redirect(referringUrl);
    }
   

}







