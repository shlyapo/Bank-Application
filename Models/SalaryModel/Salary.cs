namespace BankApp.Models.Entity.SalaryModel
{
    public class Salary
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string Profession { get; set; }
        public double Money { get; set; }
    }
}
