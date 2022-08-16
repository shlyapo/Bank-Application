using Microsoft.AspNetCore.Identity;

namespace BankApp.ViewModels
{
    public interface IPasswordValidator<T> where T : class {

        Task<IdentityResult> ValidateAsync(UserManager<T> manager, T user, string password);
    }
}
