using Microsoft.CodeAnalysis;
using OnShop.Areas.Identity.Data;
using OnShop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserProducts
{

    public UserProducts()
    {
        ProductId = 0;
       UserId = string.Empty;
    }


 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserProductId { get; set; } 
    public int ProductId { get; set; }
    [ForeignKey("ApplicationUser")]

    public string UserId { get; set; }
    
    public int CategoryId { get; set; }
    
    public string CategoryName { get; set; }

     
    public string ProductName { get; set; }
 
    public decimal Price { get; set; }
 
    public int Quantity { get; set; }
  
    public string? ImageUrl { get; set; }
    
    public string? Description { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

   
}
