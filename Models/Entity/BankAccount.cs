namespace BankApp.Models.Entity
{
    public class BankAccount//добавить время создания
    {
        public int Id { get; set; }
        //public string? BalanceName { get; set; }
        public string Name { get; set; }
        public double Money { get; set; }
        public int ClientId { get; set; }
        public int BankId { get; set; }
        public string Status { get; set; }
        // public Balance? Balance { get; set; }
        public virtual Client Client { get; set; } = null!;
        public virtual Bank? Bank { get; set; }
        public bool? Salary { get; set; }
        public bool? SalaryDay { get; set; }
        //public int BankId { get; set; }  
        public virtual ICollection<InstallmentPlan> Installments { get; set; }

        public BankAccount()
        {
            Installments = new HashSet<InstallmentPlan>();
        }
    }
}
