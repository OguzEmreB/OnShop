using OnShop.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShoppingCart
{
    [Key]
    public int ShoppingCartId { get; set; }

 
    [ForeignKey("ProductId")]
    public string ProductId { get; set; }
    
    public string ProductName { get; set; }
    
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

   
}
