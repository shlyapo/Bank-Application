using Microsoft.AspNetCore.Identity;

namespace BankApp.Models.Entity
{
    public class User:IdentityUser
    {
       // public int Id { get; set; }
        public string? IdentificationNum { get; set; }
        public int? Number { get; set; }
        public int? PassportNum { get; set; }
        public string? PassportSerier { get; set; }
        // public Role Role { get; set; }
        //public string? Passport { get; set; }
        //public string? IdPassport { get; set; }
        //public string Password { get; set; }
        //public string? Email { get; set; }
        //public int Id { get; set; }
       // public string? RoleName { get; set; }
       // public string? FirstName { get; set; }
       // public string? LastName { get; set; }
       // public string? Patronymic { get; set; }
        public virtual ICollection<Specialist> Specialists { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Manager> Managers { get; set; }
        public virtual ICollection<Operator> Operators { get; set; }
        public virtual ICollection<Admin>? Admins { get; set; }
        public User()
        {
            Clients = new HashSet<Client>();
            Specialists = new HashSet<Specialist>();
           // Actions = new HashSet<Action>();
           Operators = new HashSet<Operator>();
            Managers = new HashSet<Manager>();
            Admins = new HashSet<Admin>();
        }
        
    }
}
