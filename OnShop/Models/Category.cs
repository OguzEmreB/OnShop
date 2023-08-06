using System;
using System.Collections.Generic;

namespace OnShop.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public byte[]? Picture { get; set; }

    public string? Description { get; set; }

    public int? Quantity { get; set; }
}
  