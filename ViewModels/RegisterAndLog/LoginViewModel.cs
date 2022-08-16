using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User")]
        public string NameUser { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}