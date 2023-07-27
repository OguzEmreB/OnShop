using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnShop.Models;

namespace OnShop.Areas.Identity.Data;

 
public class ApplicationUser : IdentityUser
{

    public ApplicationUser()
    {
        ShoppingCart = new List<ShoppingCart>();
        UserProducts = new List<UserProducts>();

    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<ShoppingCart> ShoppingCart { get; set; }
    public List <UserProducts> UserProducts { get; set; }
}

    