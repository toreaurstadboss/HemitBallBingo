using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Feltet er påkrevd"));

builder.Services.AddDbContext<HemitBallbingoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lottery}/{action=Index}/{id?}");

app.Run();