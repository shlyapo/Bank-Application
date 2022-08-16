using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class ApplyCreditAndInstallment
    {
        [Required]
        [Display(Name = "Account")]
        public string BankAccount { get; set; }

        [Required]
        [Display(Name = "Sum")]
        public int Money { get; set; }

        // [Required]
        // [Display(Name = "MonthProcent")]
        public int? MoneyProcent { get; set; }

        [Required]
        [Display(Name = "Count")]
        public int Count { get; set; }

        public int? BalanceId { get; set; }
        public int ClientId { get; set; }
        public static int? BankId { get; set; }
        public string? ClientName { get; set; }
    }
}
