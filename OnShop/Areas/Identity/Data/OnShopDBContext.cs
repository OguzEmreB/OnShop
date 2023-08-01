using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnShop.Areas.Identity.Data;
using OnShop.Models;
using System.Reflection.Emit;

namespace OnShop.Data;

public class OnShopDBContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<UserProducts> UserProducts { get; set; }
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public OnShopDBContext(DbContextOptions<OnShopDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Picture).HasColumnType("image");
        });

        modelBuilder.Entity<Products>(entity =>
        {


            entity.Property(e => e.Description).HasMaxLength(120);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ProductName).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<UserProducts>()
        .Property(u => u.Price)
        .HasColumnType("decimal(18, 2)");
    }
}
