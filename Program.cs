using BankApp.Data;
using BankApp.Models.Entity;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


    

// using (var scope = builder..CreateScope())



//builder.Services.AddTransient<BankApp.ViewModels.IPasswordValidator<User>,
// CustomPasswordValidator>(serv => new CustomPasswordValidator(6));

//builder.Services.AddTransient<BankApp.ViewModels.IUserValidator<User>, CustomUserValidator>();

builder.Services.AddIdentity<User,IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
   
    // options.User.RequireUniqueEmail = true;    // уникальный email
    // options.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz";
    // options.Password.RequiredLength = 5;   // минимальная длина
    //options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
    //options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    //options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    //options.Password.RequireDigit = false;
} )
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.Run();
