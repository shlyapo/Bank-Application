
using BankApp.Models;
using BankApp.Models.Entity.SalaryModel;

namespace BankApp.Models.Entity
{
    public class Client 
    { 
        public int Id { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }= null!;
        public int CompanyId { get; set; }
        public string? Profession { get; set; }
        public double? PayForProfession { get; set; }
        public virtual ICollection<Bank>? Banks { get; set; }
        public virtual ICollection<BankAccount>? Accounts { get; set; }
       // public virtual ICollection<ClientsOfBanks>? ClientOfBanks { get; set; }
        public virtual ICollection<InstallmentPlan>? Installments { get; set; }
        //public virtual ICollection<Credit>? Credits { get; set; }
        //public virtual ICollection<Salary>? Salarys { get; set; }
        //public virtual ICollection<Company>? Companies { get; set; }
        public Client()
        {
            Banks = new HashSet<Bank>();
            Accounts = new HashSet<BankAccount>();
            Installments = new HashSet<InstallmentPlan>();
        }
    }
}
