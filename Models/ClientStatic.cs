namespace BankApp.Models.Entity
{
    public class ClientStatic
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public int ClientId { get; set; }

        public int? AccountId { get; set; }
        public double? Money { get; set; }

    }
}
