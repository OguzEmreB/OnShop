using Microsoft.EntityFrameworkCore;
using OnShop.Models;
using Microsoft.AspNetCore.Identity;
using OnShop.Data;
using OnShop.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

 
builder.Services.AddControllersWithViews();
 

builder.Services.AddDbContext<OnShopDBContext>(option => option.UseSqlServer(

    builder.Configuration.GetConnectionString("OnShopDBContextConnection")
    )); // Identity
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<OnShopDBContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders(); 

builder.Services.AddRazorPages();



var app = builder.Build();


 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
