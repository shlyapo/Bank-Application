namespace BankApp.Models.Entity
{
    public class CompanyAcc
    {
        public int Id { get; set; }

        public int BankId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public double Money { get; set; }

        public virtual Bank Bank { get; set; }

    }
}
