namespace BankApp.Models.Entity
{
    public class MoneyTrans
    {
        public int Id { get; set; }
        public int ClientId { get; set; } 
        public string Acc { get; set; }
        public int BankId { get; set; }
        public int? AccountId { get; set; }
        public int? InstallmentId { get; set; }
        public string Operation { get; set; }
        public bool Status { get; set; }
        public double Money { get; set; }
        
    }
}
