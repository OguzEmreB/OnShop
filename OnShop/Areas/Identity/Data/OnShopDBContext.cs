using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnShop.Areas.Identity.Data;

namespace OnShop.Data;

public class OnShopDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<UserProducts> UserProducts { get; set; }

    public OnShopDBContext(DbContextOptions<OnShopDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
 
    }
}
