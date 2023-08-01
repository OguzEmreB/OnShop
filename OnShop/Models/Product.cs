using OnShop.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnShop.Models;

public partial class Products
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  
    public int ProductId { get; set; } 
    public int? UserProductId { get; set; }
    public int? CategoryId { get; set; }
   
    public string? CategoryName { get; set; }
     

    public string ProductName { get; set; } = null!;
     

    public string? Description { get; set; }
   

    public decimal Price { get; set; }
    

    public int? Quantity { get; set; }
     
    public string? ImageUrl { get; set; }
    [ForeignKey("ApplicationUser")]
    public string? UserId { get; set; }
 

    public virtual Category? Category { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    public virtual ICollection<UserProducts> UserProducts { get; set; }


}