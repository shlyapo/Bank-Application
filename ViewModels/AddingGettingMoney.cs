using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class AddingGettingMoney
    {
        static public int Id { get; set; }
        [Required]
        [Display(Name = "Money")]
        public string Money{ get; set; }
    }
}

