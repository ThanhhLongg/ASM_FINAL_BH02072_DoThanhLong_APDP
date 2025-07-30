using MvcAdoDemo.Models; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Cấu hình DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSession(); // Thêm dòng này

// Thêm MVC
builder.Services.AddControllersWithViews();

// ✅ Bật session ở đây
builder.Services.AddSession();

var app = builder.Build();
app.UseSession(); // bật session

// Middleware...
app.UseStaticFiles();
app.UseRouting();

// ✅ Bật session ở đây
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
