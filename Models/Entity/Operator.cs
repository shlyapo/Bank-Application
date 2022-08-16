namespace BankApp.Models.Entity
{
    public class Operator
    {
        public int BankId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public virtual Bank Bank { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        //public string? Email { get; set; }
        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }
        // public int ClientId { get; set; }  
        // public bool Trans { get; set; }
        ////public virtual ICollection<Client>? SalaryClients { get; set; }
        //public virtual ICollection<Client>? Clients { get; set; }
        public Operator()
        { }
    }
}