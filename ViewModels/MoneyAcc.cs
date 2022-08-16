using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class MoneyAcc
    {
        [Required]
        [Display(Name ="Money")]
        public string Money { get; set; }
        static public int Id { get; set; }
        
    }
}
