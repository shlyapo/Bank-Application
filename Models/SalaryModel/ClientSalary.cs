namespace BankApp.Models.Entity.SalaryModel
{
    public class ClientSalary
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int SalaryId { get; set; }
        public int ClientId { get; set; }
        public virtual Company Company { get; set; }
    }
}
