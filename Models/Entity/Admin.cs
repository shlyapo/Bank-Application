namespace BankApp.Models.Entity
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public Admin() { }
    }
}
