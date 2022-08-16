using BankApp.Data;
using BankApp.Models.Entity;
using BankApp.Models.Entity.SalaryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Controllers
{
        [Authorize(Roles = "Specialist")]
        public class SpecialistController : Controller
    { 
            UserManager<User> _userManager;
            ApplicationDbContext _context;

            public SpecialistController(UserManager<User> userManager, ApplicationDbContext context)
            {
                _userManager = userManager;
                _context = context;

            }
            public IActionResult Index()
            {
                return View();
            }

        public IActionResult ChooseClients()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithCompany = _context.Users.Where(x => x.UserName == userName).Include(x => x.Specialists).Single();
            Specialist spec = userWithCompany.Specialists.First();
            var SpectWithCompany = _context.Specialists.Where(x => x.Id == spec.Id).Include(x => x.Company).Single();
            Company comp = _context.Companies.Find(SpectWithCompany.Company.Id);
            var entWithSalaryProjects = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.Salarys).Single();
            var specialistWithBank = _context.Specialists.Where(x => x.Id == spec.Id).Include(x => x.Bank).Single();
            Bank bank = specialistWithBank.Bank;
            var bankWithClient = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.Clients).Single();
            
            //var compWithSalary = _context.Companies.Where(x => x.Id == spec.Id).Include(x => x.Salarys).SingleOrDefault();
            List<User> user = new();
            Dictionary<List<User>, List<Salary>> clientComp = new();
            foreach (var client in bankWithClient.Clients.ToList())
            {
                if (client.Profession == null)
                {
                    var clientWithUser = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                    user.Add(clientWithUser.User);
                }
            }
            clientComp.Add(user, entWithSalaryProjects.Salarys.ToList());
            return View(clientComp);

        }
        /*

            public IActionResult Sum(string Id)
            {
                MoneyTrans.ClientId = Id;

                return View();

            }
        /*
            public async Task<IActionResult> CreateRequestForCompany(MoneyTrans trans)
            {
                if (ModelState.IsValid)
                {
                    Client client = await _context.Clients.FindAsync(MoneyTrans.ClientId);
                    var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).SingleOrDefault();
                    BankAccount account = new();
                    foreach (var acc in clientWithAccounts.Accounts.ToList())
                    {
                        if (acc.Name == trans.Acc)
                        {
                            account = acc;
                        }
                    }
                    if (account.Name != trans.Acc)
                    {
                        return View("WrongAccountName");//////////////////////
                    }

                    string userName = HttpContext.User.Identity.Name;
                    var userWithComp = _context.Users.Where(x => x.UserName == userName).Include(x => x.Specialists).Single();
                    Specialist spec = userWithComp.Specialists.First();
                    var speicalistWithComp = _context.Specialists.Where(x => x.Id == spec.Id).Include(x => x.Company).Single();
                    MoneyRequest transaction = new() { SpecialistId = speicalistWithComp.CompanyId,
                                                       ClientId = client.Id, 
                                                       Money = trans.Money, AccountId = account.Id };
                    _context.MoneyRequests.Add(transaction);
                    _context.SpecStatistics.Add(new SpecStatistic() 
                    { NameOperation = "CreateRequestForEnterpriseTransaction", 
                     MoneyTransactionRequest = transaction.Id, SpecialistId = spec.Id });
                    _context.SaveChanges();
                }
                else
                {
                    return View("Views/EnterpriseSpecialist/SumForSelectedClient.cshtml", trans);
                }

                return View();
            }
        */
        /*
            public IActionResult ChooseClients()
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.Specialists).Single();
                Specialist specialist = userWithEnterprise.Specialists.First();
                var speicalistWithComp = _context.Specialists.Where(x => x.Id == specialist.Id).Include(x => x.Company).Single();
                Company ent = _context.Companies.Find(speicalistWithComp.Company.Id);

                var compWithSalary = _context.Companies.Where(x => x.Id == ent.Id).Include(x => x.Salarys).Single();
                var specialistWithBank = _context.Specialists.Where(x => x.Id == specialist.Id).Include(x => x.Bank).Single();
                Bank bank = specialistWithBank.Bank;
                var bankWithClients = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.Clients).Single();
                Dictionary<List<User>, List<Salary>> userSalary = new();
                List<User> clientsWithoutProfession = new();////////
                var li = _context.Salarys.ToList();
                foreach (var client in bankWithClients.Clients.ToList())
                {
                    if (client.Profession == null)
                    {
                        var clientWithUser = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                        clientsWithoutProfession.Add(clientWithUser.User);
                    }
                }
                userSalary.Add(clientsWithoutProfession, compWithSalary.Salarys.ToList());
                return View(userSalary);
            }*/

            public async Task<IActionResult> SelectClient(string id, int SalaryId)
            {

                User user = await _userManager.FindByIdAsync(id);
                var userWithClientts = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Clients).Single();
                string userName = HttpContext.User.Identity.Name;
                var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.Specialists).Single();
                Specialist specialist = userWithEnterprise.Specialists.First();
                var specialistWithEnterprise = _context.Specialists.Where(x => x.Id == specialist.Id).Include(x => x.Company).Single();
                Company pr = specialistWithEnterprise.Company;
                var prWithClients = _context.Companies.Where(x => x.Id == pr.Id).Include(x => x.ClientSalaries).Single();

                foreach (var cl in _context.ClientSalaries.ToList())
                {
                    if (cl.ClientId == userWithClientts.Clients.First().Id)
                    {
                        return View("ClientEnterprise");
                    }
                }
                ClientSalary salary = new() { ClientId = userWithClientts.Clients.First().Id, 
                    Company = specialistWithEnterprise.Company, 
                    SalaryId = SalaryId };
                specialistWithEnterprise.Company.ClientSalaries.Add(salary);
                _context.ClientSalaries.Add(salary);
                _context.SpecStatistics.Add(new SpecStatistic()
                { Operation = "AddClientToEnterprise", 
                    ClientSalary = salary.Id, 
                    SpecialistId = specialist.Id });
                _context.SaveChanges();
                return View();
            }

            public IActionResult WatchEnterpriseWorkers()
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithEnterprise = _context.Users.Where(x => x.UserName == userName).Include(x => x.Specialists).Single();
                Specialist specialist = userWithEnterprise.Specialists.First();
                var specialistWithBank = _context.Specialists.Where(x => x.Id == specialist.Id).Include(x => x.Bank).Single();
                Bank bank = specialistWithBank.Bank;
                var bankWithClients = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.Clients).Single();
                List<Client> clientsWithProfession = new();
                foreach (var client in bankWithClients.Clients.ToList())
                {
                    if (client.Profession != null)//////////////////
                    {
                        clientsWithProfession.Add(client);
                    }
                }
                return View(clientsWithProfession);
            }
        }
}
