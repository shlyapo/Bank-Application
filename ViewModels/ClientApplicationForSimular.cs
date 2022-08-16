using BankApp.Models.Entity;
using BankApp.Models.Entity.SalaryModel;

namespace BankApp.ViewModels
{
    public class ClientApplicationForSimular
    {
        public Dictionary<User, Salary> UserSalary { get; set; } = new();
        public List<SalaryProject> Applications { get; set; } = new();
    }
}
