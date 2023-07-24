using System;
using System.Collections.Generic;

namespace OnShop.Models;

public partial class Products
{
    public string ProductId { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? Quantity { get; set; }
    public string? ImageUrl { get; set; }

    public virtual Category? Category { get; set; }
}