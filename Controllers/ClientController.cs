using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankApp.Data;
using BankApp.ViewModels;
using BankApp.Models.Entity;
using Microsoft.AspNetCore.Identity;
using BankApp.Models.Entity.SalaryModel;

namespace BankApp.Controllers
{
    public class ClientController : Controller
    {
        ApplicationDbContext _context;

        UserManager<User> userManager;
        public ClientController(ApplicationDbContext context, UserManager<User> _userManager)
        {
            _context = context;
            userManager = _userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Bank()
        {
            string userName = HttpContext.User.Identity.Name;
            User name = await userManager.FindByNameAsync(userName);
            var userWithClient = _context.Users.Where(x => x.Id == name.Id).Include(x => x.Clients).SingleOrDefault();
            Client client = userWithClient.Clients.ToList()[0];
            var clientWithBanks = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Banks).SingleOrDefault();
            List<Bank> banks;
            banks = clientWithBanks.Banks.ToList();
            return View(banks);
        }

        [HttpGet]
        public async Task<IActionResult> AddBank(int Id)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Banks).Single();
            List<Bank> bnkList = new();

            foreach (var bnk in _context.Banks.ToList())
            {
                if (!clientWithBanks.Banks.Contains(bnk))
                {
                    bnkList.Add(bnk)
;
                }
            }
            if (bnkList.Count == 0)
            {
                return RedirectToAction("Index", "Client");
            }
            return View(bnkList);
        }

        [HttpPost]
        public async Task<IActionResult> AddBank(string Id)
        {

            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();

            Bank bnk = BankRepository.FindByName(_context, Id);

            UserCreation creation = new() { clId = client.Id, bankId = bnk.Id, ClientName = userName, Email = userWithClient.Email, Number = (string)userWithClient.PhoneNumber, PassportSerier = userWithClient.PassportSerier, PassportNum = (int)userWithClient.PassportNum, IdentificationNum = (string)userWithClient.IdentificationNum };
            foreach (var us in _context.UserCreations.ToList())
            {
                if (us.clId == creation.clId && us.bankId == creation.bankId)
                {
                    return View("Choice");
                }
            }
            await _context.UserCreations.AddAsync(creation);
            _context.SaveChanges();

            return View("Choice");
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /*public IActionResult Salary(string value)
        {
            return View();
        }*/
        
        public async Task<IActionResult> ChooseBankSalaryProject()
        {
             string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithBanks = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Banks).Single();

            if (clientWithBanks.Banks.ToList().Count == 0)
            {
                return View("Views/Client/EmptyBanks.cshtml");
            }
            return View(clientWithBanks.Banks.ToList());
        }

        public async Task<IActionResult> SelectEnterpriseForSalaryProject(int BankId)
        {
            Bank bnk = await _context.Banks.FindAsync(BankId);
            var bnkWithEnterprises = _context.Banks.Where(x => x.Id == bnk.Id).Include(x => x.Companys).Single();


            return View(bnkWithEnterprises.Companys.ToList());
        }

        public async Task<IActionResult> SelectSalaryProject(int CompanyId)
        {
            Company ent = await _context.Companies.FindAsync(CompanyId);
            var entWithSalary = _context.Companies.Where(x => x.Id == ent.Id).Include(x => x.Salarys).Single();
            Dictionary<List<Salary>, int> salaryEnterprise = new();
            salaryEnterprise.Add(entWithSalary.Salarys.ToList(), CompanyId);

            return View(salaryEnterprise);

        }

        public async Task<IActionResult> SalaryProjectApplication(int SalaryId, int CompanyId)
        {
            var salary = _context.Salarys.Find(SalaryId);
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();


            if (client.CompanyId != null)
            {
                Company comp = _context.Companies.Find(client.CompanyId);
                return View("Views/Client/RejectSalary.cshtml", comp);///
            }

            foreach (var el in _context.SalaryProjects.ToList())
            {
                if (el.ClientId == client.Id)
                {
                    return View("Views/Client/SalaryProjectApplication.cshtml");
                }
            }
            Company company = _context.Companies.Find(CompanyId);
            var compWithBank = _context.Companies.Where(x => x.Id == company.Id).Include(x => x.Bank).Single();
            Bank bnk = compWithBank.Bank;
            SalaryProject application = new() { ClientId = client.Id, SalaryId = SalaryId, Bank = bnk };
            _context.SalaryProjects.Add(application);
            bnk.SalaryProjects.Add(application);
            _context.SaveChanges();

            return View("Views/Client/SalaryProjectApplication.cshtml");


        }

    }
}
