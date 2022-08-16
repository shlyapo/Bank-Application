namespace BankApp.Models.Entity
{
    public class Specialist
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int CompanyId { get; set; }
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; } = null!;
        public virtual User User { get; set; }= null!;
        public virtual Company Company { get; set; } = null!;
        //public virtual ICollection<Balance>? Balances { get; set; }
        public Specialist() { }
    }
}
