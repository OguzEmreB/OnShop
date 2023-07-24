
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
    // Örnek: Sepetteki ürünleri listeleme işlemi
    [HttpGet]
    public IActionResult ListItemsInCart()
    {
        // GetCartProducts metodunu çağırarak verileri al
        var productsInCart = GetCartProducts();

        // ViewBag'de ürünleri listeyi tutalım
        ViewBag.ProductsInCart = productsInCart;

        // Toplam fiyatı hesaplamak için değişken oluşturalım
        decimal totalPrice = 0;

        // Ürünlerin Price özelliğini toplayarak toplam fiyatı hesaplayalım
        foreach (var product in productsInCart)
        {
            totalPrice += product.Price *product.Quantity;
        }

        // ViewBag ile toplam fiyatı da view'e gönderelim
        ViewBag.TotalPrice = totalPrice;




        return View();
    }

    public List<ShoppingCart> GetCartProducts()
    {
        // Kullanıcının kimlik kimliğine göre ApplicationUser'ı alalım
        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

        if (user == null)
        {
            // Hata durumunda boş liste döndürebiliriz
            return new List<ShoppingCart>();
        }

        // Kullanıcının sepetindeki ürünleri getirelim
        var productsInCart = _dbContext.ShoppingCarts
            .Where(cart => cart.ApplicationUser.Id == userId)
            .ToList();

        return productsInCart;
    }







    [Authorize]
    [HttpPost]
    // Örnek: Sepete ürün ekleme işlemi
    public IActionResult AddToCart(string productId, int quantity)
    {
        // Kullanıcının kimlik kimliğine göre ApplicationUser'ı alalım
        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

        // Eğer kullanıcı henüz sepet oluşturmadıysa yeni bir sepet oluşturalım
        if (user.ShoppingCart == null)
        {
            user.ShoppingCart = new List<ShoppingCart>();
        }

        // Ürünü veritabanından alalım (örneğin ProductId kullanılarak)
        var productToAdd = _dbContextProduct.Products.FirstOrDefault(p => p.ProductId == productId);

        // Eğer ürün varsa ve kullanıcının sepetinde henüz yoksa ekleyelim
        if (productToAdd != null && !user.ShoppingCart.Any(item => item.ProductId == productToAdd.ProductId))
        {
            // ShoppingCart nesnesini oluşturalım ve ürünü ekleyelim
            var shoppingCartItem = new ShoppingCart
            {
                ProductId = productToAdd.ProductId,
                ProductName = productToAdd.ProductName, // Set the ProductName property explicitly
                Price=productToAdd.Price,// Set the Price property explicitly
                Quantity = quantity // Quantity değerini de belirtiyoruz
                                    // Diğer özellikleri gerekirse burada ayarlayabilirsiniz...
            };
            user.ShoppingCart.Add(shoppingCartItem);
        }

        // ApplicationUser nesnesini veritabanına kaydedelim
        _userManager.UpdateAsync(user).Wait();

        // Sepete ekledikten sonra, doğrudan ListItemsInCart sayfasına yönlendirelim
        return RedirectToAction("ListItemsInCart");
    }
     

    [Authorize]
    [HttpPost]
    // Sepeti temizleme işlemi
    public IActionResult ClearCart()
    {
        var userId = _userManager.GetUserId(User);
        var user = _userManager.FindByIdAsync(userId).Result;

        if (user != null)
        {
            // Veritabanından kullanıcının cart verilerini doğrudan sil
            var userCartItems = _dbContext.ShoppingCarts.Where(cart => cart.ApplicationUser.Id == userId);
            _dbContext.ShoppingCarts.RemoveRange(userCartItems);
            _dbContext.SaveChanges();

            // Kullanıcının ShoppingCart listesini de boşalt
            user.ShoppingCart.Clear();

            // ApplicationUser nesnesini veritabanına kaydet
            _userManager.UpdateAsync(user).Wait();
        }

        return RedirectToAction("ListItemsInCart");
    }


}







