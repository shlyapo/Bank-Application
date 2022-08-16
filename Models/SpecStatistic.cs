namespace BankApp.Models.Entity
{
    public class SpecStatistic
    {
        public int Id { get; set; }
        public int SpecialistId { get; set; }
        public string Operation { get; set; }
        public int? MoneyRequest { get; set; }
        public int? ClientSalary { get; set; }
    }
}
