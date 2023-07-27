using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnShop.Models;

public partial class OnShopContext : DbContext
{
    public OnShopContext()
    {
    }

    public OnShopContext(DbContextOptions<OnShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Products> Products { get; set; }
 



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=OnShop;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Picture).HasColumnType("image");
        });

        modelBuilder.Entity<Products>(entity =>
        {
           

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ProductName).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
