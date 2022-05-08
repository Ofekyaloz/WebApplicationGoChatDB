using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplicationGoChat.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebApplicationGoChatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplicationGoChatContext") ?? throw new InvalidOperationException("Connection string 'WebApplicationGoChatContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();
