namespace BankApp.Models.InstallmentOrCreditModel
{
    public class CreateInstallment
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int Money { get; set; }
        public int? MoneyProcent { get; set; }
        public int Count { get; set; }
        public string ClientName { get; set; }
         public int ClientId { get; set; }
        public int BankId { get; set; }
        //public DateTime OpenTime { get; set; }
    }
}
