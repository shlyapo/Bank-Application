

namespace BankApp.Models.Entity
{

    public class Manager
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public int BankId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Bank Bank { get; set; } = null!;
       
    }
}
