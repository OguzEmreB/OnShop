using Microsoft.AspNetCore.Mvc.Rendering;
using OnShop.Areas.Identity.Data;

public class AddProductViewModel
{
    public string ProductName { get; set; }

    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }

    public List<SelectListItem> Categories { get; set; }

  
    public AddProductViewModel()
    {
      
        Categories = new List<SelectListItem>();
    }


}
 
