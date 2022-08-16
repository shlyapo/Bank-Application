using BankApp.Data;
using Microsoft.AspNetCore.Mvc;
using BankApp.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BankApp.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public BankAccountController(ApplicationDbContext context, UserManager<User> userManager)
        { 
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankAccounts.ToListAsync());
        }

        public async Task<IActionResult> ChooseBankAccount()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client name = userWithClient.Clients.First();
            var clientWithBanks = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Banks).SingleOrDefault();
            //ViewBag.Banks = new SelectList(_context.Banks, "Id", "Name");
            if(clientWithBanks.Banks.ToList().Count == 0)
            {
                return View("EmptyBanks");
            }
            return View("Views/Client/BankAccount/ChooseBankAccount.cshtml", clientWithBanks.Banks.ToList());
        }

        
        [HttpGet]
        public async Task<IActionResult> Account(int bankId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client name = userWithClient.Clients.First();
            Bank bank = await _context.Banks.FindAsync(bankId);
            var clientWithAcc = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Accounts).SingleOrDefault();
            List<BankAccount> accounts = new();
            Dictionary<List<BankAccount>, int> accWithBank = new();
            foreach (var acc in clientWithAcc.Accounts.ToList())
            {
                if (acc.Bank== bank)
                    accounts.Add(acc);
            }
            ///прописать счёт блокировку
            accWithBank.Add(accounts, bankId);
            return View("Views/Client/BankAccount/Account.cshtml", accWithBank);
        }

        [HttpGet]
        public ActionResult Create( int bankId)
        {
            Bank bank = _context.Banks.Find(bankId);
            CreateAcc.Bank = bank;
            return View("Views/Client/BankAccount/BankAccountCreate.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAcc createAcc)
        {
            uint current;
            if (!UInt32.TryParse(createAcc.Money, out current))
            {
                return View(createAcc);
            }
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client name = userWithClient.Clients.First();
            //Client client = userWithClient.Clients.ToList()[0];
            var clientWithBanks = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Banks).SingleOrDefault();
            //List<Bank> banks;
            //banks = clientWithBanks.Banks.ToList();
            Bank bank =  _context.Banks.Find(CreateAcc.Bank.Id);
          
            var clientWithAcc = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Accounts).SingleOrDefault();

            if (ModelState.IsValid)
            {
                foreach (var acc in clientWithAcc.Installments.ToList())
                {
                    if (acc.Name == createAcc.Name && acc.Bank == bank)
                        return View("Views/Client/BankAccount/BankAccountCreate.cshtml", createAcc);
                }
                BankAccount account = new BankAccount
                {
                    Name = createAcc.Name,
                    Money = current,
                    Client = name,
                    Bank = bank,
                    Status = "Free"
                };
                await _context.BankAccounts.AddAsync(account);
                bank.Accounts.Add(account);
                _context.SaveChanges();
                return View("Views/Client/Index.cshtml");

            }
            else 
                return View("Views/Client/BankAccount/BankAccountCreate.cshtml",createAcc);
        }

        [HttpGet]
        public async Task<IActionResult> AddSum(int id)
        {
            MoneyAcc.Id = id;
            return View("Views/Client/BankAccount/AddSumToAccount.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddSum(MoneyAcc mon)
        {
            uint current;
            if (!UInt32.TryParse(mon.Money, out current))
            {
                return View("Views/Client/BankAccount/AddSumToAccount.cshtml",mon);

            }
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).SingleOrDefault();
                Client client = userWithClient.Clients.First();
                var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).SingleOrDefault();

                var acc = _context.BankAccounts.Find(MoneyAcc.Id);
                acc.Money += (int)current;
                var accountWithBank = _context.BankAccounts.Where(x => x.Id == acc.Id).Include(x => x.Bank).Single();
                MoneyStatistic money = new()
                {
                    Operation = "AddMoneyAcc",
                    Status = true,
                    Money = (int)current,
                    ClientId = userWithClient.Clients.First().Id,
                    BankId = accountWithBank.BankId,
                    AccountId = acc.Id
                };
                _context.MoneyStatistics.Add(money);
                _context.SaveChanges();
            }
            else
                return View("Views/Client/BankAccount/AddSumToAccount.cshtml", mon);

            return View("Views/Client/Index.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> GetSum(int id)
        {
            MoneyAcc.Id = id;
            return View("Views/Client/BankAccount/TakeSumFromAccount.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> GetSum(MoneyAcc mon)
        {
            uint current;
            if (!UInt32.TryParse(mon.Money, out current))
            {
                return View("Views/Client/BankAccount/TakeSumFromAccount.cshtml", mon);

            }
            if (ModelState.IsValid)
            {
                var account = _context.BankAccounts.Find(MoneyAcc.Id);
                if (account.Money < current)
                {
                    return View("Views/Client/BankAccount/TakeSumFromAccount.cshtml", mon);
                }
                account.Money -= (int)current;
                _context.SaveChanges();
            }

            else
                return View("Views/Client/BankAccount/TakeSumFromAccount.cshtml", mon);
            return View("Views/Client/Index.cshtml");

        }

        public async Task<IActionResult> Free(int id)
        {
            BankAccount acc = _context.BankAccounts.Find(id);
            acc.Status = "Free";
            _context.SaveChanges();
            return View("Views/Client/Index.cshtml");
        } 

        public async Task<IActionResult> Freeze(int id)
        {
            BankAccount acc = _context.BankAccounts.Find(id);
            acc.Status = "Freeze";
            _context.SaveChanges();
            return View("Views/Client/Index.cshtml");
        }


        public async Task<IActionResult> Close(int id)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).SingleOrDefault();
            Client client = userWithClient.Clients.First();
            var acc = await _context.BankAccounts.FindAsync(id);
            Bank bank = await _context.Banks.FindAsync(acc.BankId);
            _context.BankAccounts.Remove(acc);
            var clientwithAcc = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).SingleOrDefault();
            if (clientwithAcc.Accounts.Contains(acc))
                client.Accounts.Remove(acc);

            bank.Accounts.Remove(acc);
            _context.SaveChanges();
            return View("Views/Client/Index.cshtml");

        }

        public async Task<IActionResult> Redact(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);
            return View("Views/Client/BankAccount/Redact.cshtml",account);
        }
    }
}
