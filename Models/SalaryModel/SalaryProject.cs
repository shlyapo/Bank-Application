namespace BankApp.Models.Entity.SalaryModel
{

    public class SalaryProject
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }
        public int SalaryId { get; set; }
        public string? Simular { get; set; }
    }
}
