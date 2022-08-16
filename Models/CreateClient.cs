namespace BankApp.Models
{
    public class CreateClient
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int BankId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string IdentificationNum { get; set; }
        public int PassportNum { get; set; }
        public string PassportSerier { get; set; }
    }
}
