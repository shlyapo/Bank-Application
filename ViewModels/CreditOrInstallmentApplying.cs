using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class CreditOrInstallmentApplying
    {
        [Required]
        [Display(Name = "Bank account")]
        public string BankAccount { get; set; }

        [Required]
        [Display(Name = "Money")]
        public double Money { get; set; }

        // [Required]
        // [Display(Name = "MonthProcent")]
        public int? MoneyProcent { get; set; }

        [Required]
        [Display(Name = "Amount of month")]
        public int Count { get; set; }

        public int? AccountId { get; set; }
        public int? ClientId { get; set; }
        public static int? BankId { get; set; }
        public string? ClientName { get; set; }
    }
}
