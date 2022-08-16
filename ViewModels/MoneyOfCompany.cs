using BankApp.Models.Entity;

namespace BankApp.ViewModels
{
    public class MoneyOfCompany
    {
        public string[] Companies { get; set; } = new string[100];
        public double[] Money { get; set; } = new double[100];
        public Dictionary<Client, User> ClientUsers { get; set; } = new();
        public BankAccount[] Accounts { get; set; } = new BankAccount[100];
        //public int[] MoneyTransactionRequestId { get; set; } = new int[100];
    }
}
