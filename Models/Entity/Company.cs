using BankApp.Models.Entity.SalaryModel;

namespace BankApp.Models.Entity
{
    public class Company
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string TYP { get; set; }

        public string YrName { get; set; }

        public string BIK { get; set; }

        public string YrAdress { get; set; }
        //public int SalaryMoney { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual ICollection<Salary> Salarys { get; set; }
        public virtual ICollection<Specialist> Specialists { get; set; }
        public virtual ICollection<ClientSalary> ClientSalaries { get; set; }
        //public virtual ICollection<Bank> Banks { get; set; }
        public Company()
        {
            Salarys = new HashSet<Salary>();
            Specialists = new HashSet<Specialist>();
            ClientSalaries = new HashSet<ClientSalary>();
        }
    }
}
