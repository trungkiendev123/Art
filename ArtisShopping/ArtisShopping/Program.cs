using ArtisShopping.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Partials/{0}" + RazorViewEngine.ViewExtension);
});
builder.Services.AddDbContext<artisContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddScoped<artisContext>();

builder.Services.AddRazorPages();

// Register CategoriesModel as a transient service
builder.Services.AddTransient<ArtisShopping.Pages.Shared.CategoriesModel>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
