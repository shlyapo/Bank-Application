using BankApp.Models;
using BankApp.Models.Entity.SalaryModel;

namespace BankApp.Models.Entity
{
    public class Bank
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public int? BankId { get; set; } 

        public virtual ICollection<Client>? Clients { get; set; }
        public virtual ICollection<BankAccount>? Accounts { get; set; }
        //public virtual ICollection<ClientsOfBanks>? ClientOfBanks { get; set; }
        public virtual ICollection<InstallmentPlan>? Installments { get; set; }
        public virtual ICollection<Credit>? Credits { get; set; }
        public virtual ICollection<Salary>? Salarys { get; set; }
        public virtual ICollection<Company>? Companys { get; set; }
        public virtual ICollection<Manager>? Managers { get; set; }
        public virtual ICollection<Specialist> Specialists { get; set; }
        public virtual ICollection<Operator> Operators { get; set; }
        public virtual ICollection<SalaryProject> SalaryProjects { get; set; }
        public Bank()
        {
            Clients=new HashSet<Client>();
            Accounts=new HashSet<BankAccount>();    
            Operators= new HashSet<Operator>();
            SalaryProjects=new HashSet<SalaryProject>();
            Installments= new HashSet<InstallmentPlan>();
            Credits = new HashSet<Credit>();
            Salarys=new HashSet<Salary>();
            Companys=new HashSet<Company>();
            Managers=new HashSet<Manager>();
            Specialists=new HashSet<Specialist>();
        }
    }
}
