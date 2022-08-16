namespace BankApp.Models.Entity
{
    public class MoneyStatistic
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int BankId { get; set; }
        public string Operation { get; set; }
        public bool Status { get; set; }
        public int Money { get; set; }
        public int? AccountId { get; set; }
        public int? InstallmentId { get; set; }
    }
}
