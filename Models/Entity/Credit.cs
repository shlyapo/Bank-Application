using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Models.Entity
{
    
    public class Credit:InstallmentPlan ///проценты добавить
    {
       //public double Procent { get; set; }
       public double MonthProcent { get; set; }

    }
}
