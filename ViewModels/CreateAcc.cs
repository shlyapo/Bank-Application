using System.ComponentModel.DataAnnotations;
using BankApp.Models.Entity;

namespace BankApp.ViewModels
{
    public class CreateAcc
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Money")]
        public string Money { get; set; }

        public static Bank Bank { get; set; }
    }
}
