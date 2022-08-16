using BankApp.Data;
using Microsoft.EntityFrameworkCore;
using BankApp.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace BankApp.Data
{
    public class Initializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {

            if (await roleManager.FindByNameAsync("Client") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }

            if (await roleManager.FindByNameAsync("Operator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Operator"));
            }

            if (await roleManager.FindByNameAsync("Manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            if (await roleManager.FindByNameAsync("Specialist") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Specialist"));
            }

            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            
        }
    }
}
