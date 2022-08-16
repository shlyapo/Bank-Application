using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class RegistrationUser
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        // [Required]
        // [Display(Name = "Год рождения")]
        // public int Year { get; set; }
        [Required]
        [Display(Name = "Number")]
        public string Number { get; set; }

        [Required]
        [Display(Name = "Passport serier")]
        public string PassportSerier { get; set; }

        [Required]
        [Display(Name = "Passport number")]
        public int PassportNum { get; set; }

        [Required]
        [Display(Name = "Identification number")]
        public string IdentificationNum { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}