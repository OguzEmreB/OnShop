
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
 
public class ShoppingCartController : Controller
{
    private readonly OnShopDBContext _dbContext;
    private readonly OnShopContext _dbContextProduct;
    private readonly UserManager<ApplicationUser> _userManager;

    public ShoppingCartController(OnShopDBContext dbContext, OnShopContext dbContextProduct, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _dbContextProduct = dbContextProduct;
        _userManager = userManager;
    }
    [Authorize]
    
    [HttpGet]
    public IActionResult ListItemsInCart()
    {
        PopulateCartProductDataInViewBag();

        var productsInCart = GetCartProducts();

     
        ViewBag.ProductsInCart = productsInCart;

       
        decimal totalPrice = 0;

    
        foreach (var product in productsInCart)
        {
            totalPrice += product.Price *product.Quantity;
        }

      
        ViewBag.TotalPrice = totalPrice;




        return View();
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
    [HttpPost]
  
    public IActionResult AddToCart(string productId, int quantity)
    {
         
        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

    
        if (user.ShoppingCart == null)
        {
            user.ShoppingCart = new List<ShoppingCart>();
        }

   
        var productToAdd = _dbContextProduct.Products.FirstOrDefault(p => p.ProductId == productId);

        
        if (productToAdd != null && !user.ShoppingCart.Any(item => item.ProductId == productToAdd.ProductId))
        {
     
            var shoppingCartItem = new ShoppingCart
            {
                ProductId = productToAdd.ProductId,
                ProductName = productToAdd.ProductName, 
                Price=productToAdd.Price, 
                Quantity = quantity  
                                
            };
            user.ShoppingCart.Add(shoppingCartItem);
        }

    
        _userManager.UpdateAsync(user).Wait();

      
        return RedirectToAction("ListItemsInCart");
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

        return RedirectToAction("ListItemsInCart");
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







