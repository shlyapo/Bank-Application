namespace BankApp.Models.Entity
{
    public class Rolea
    { 
            public int Id { get; set; }
            public string Name { get; set; }
            public List<User> Users { get; set; }
            public Rolea()
            {
                Users = new List<User>();
            }
    }
}
