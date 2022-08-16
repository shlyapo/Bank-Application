
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankApp.Models.Entity;
using BankApp.Data;

namespace BankApp.ViewModels
{
    public class SendSalary
    {
        static UserManager<User> _userManager;
        static RoleManager<User> _roleManager;
        static ApplicationDbContext _context;
        static List<Bank> banks { get; set; } = new();
        public static async Task Initialize(UserManager<User> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;

            if (DateTime.Now.Day == 25)
            {
                foreach (var bnk in _context.Banks.ToList())
                {
                    var bnkWithClients = _context.Banks.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
                    foreach (var client in bnkWithClients.Clients.ToList())
                    {
                        if (client.Profession != null)
                        {
                            var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                            foreach (var acc in clientWithAccounts.Accounts.ToList())
                            {
                                if (acc.Name == client.Profession && acc.SalaryDay != true)
                                {
                                    acc.Money += (double)client.PayForProfession;
                                    acc.SalaryDay = true;
                                }
                            }
                        }
                    }
                    _context.SaveChanges();
                }

            }
            if (!(DateTime.Now.Day == 25))
            {
                foreach (var bnk in _context.Banks.ToList())
                {
                    var bnkWithClients = _context.Banks.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
                    foreach (var client in bnkWithClients.Clients.ToList())
                    {
                        if (client.Profession != null)
                        {
                            var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
                            foreach (var acc in clientWithAccounts.Accounts.ToList())
                            {
                                if (acc.Name == client.Profession && acc.SalaryDay == true)
                                {

                                    acc.SalaryDay = false;
                                }
                            }
                        }
                    }

                }
                _context.SaveChanges();
            }

        }
    }
}