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

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public string? UserId { get; set; }

    public virtual Category? Category { get; set; }

}