using BankApp.Data;
using BankApp.Models.Entity;
using BankApp.Models.InstallmentOrCreditModel;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Controllers
{
   
    public class CreditController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public CreditController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Choose()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client name = userWithClient.Clients.First();
            var clientWithBanks = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Banks).SingleOrDefault();
            //ViewBag.Banks = new SelectList(_context.Banks, "Id", "Name");
            if (clientWithBanks.Banks.ToList().Count == 0)
            {
                return View("Views/Client/EmptyBanks.cshml");
            }
            return View("Views/Client/Credit/BankCredit.cshtml", clientWithBanks.Banks.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> Credit(int BankId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client client = userWithClient.Clients.First();
            var ClientWithCredits = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Installments).Single();
            Dictionary<List<Credit>, int> creditsWithBank = new();
            List<Credit> credits = new();
            foreach (var credit in ClientWithCredits.Installments.ToList())
            {
                if (credit is Credit cr && credit.Status != false)
                {
                    credits.Add(cr);
                }
            }
            foreach (var credit in credits)
            {
                if (DateTime.Now > credit.CloseTime)
                {
                    var creditWithAccounts = _context.Installments.Where(x => x.Id == credit.Id).Include(x => x.BankAccount).Single();
                    credit.BankAccount.Status = "Block";
                }

            }
            creditsWithBank.Add(credits, BankId);

            return View("Views/Client/Credit/Credit.cshtml", creditsWithBank);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCredit(int bankId)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            Client name = userWithClient.Clients.First();
            //Client client = userWithClient.Clients.ToList()[0];
            var clientWithBanks = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Banks).SingleOrDefault();
            //List<Bank> banks;
            //banks = clientWithBanks.Banks.ToList();
            //Bank? bank = await _context.Banks.FirstOrDefaultAsync(p => p.Id == bankId);
            //BankAccount account = new BankAccount()
            var clientWithCredits = _context.Clients.Where(x => x.Id == name.Id).Include(x => x.Installments).SingleOrDefault();
            List<Credit> credits = new();
            Dictionary<List<Credit>, int> creditWithBank = new();
            foreach(var credit in clientWithCredits.Installments.ToList())
            {
                if (credit is Credit cred)
                    if (credit.Status==true)
                    {
                        credits.Add(cred);
                    }
            }
            ///прописать счёт блокировку
            foreach (var credit in credits)
            {
                if (DateTime.Now > credit.CloseTime)
                {
                    var creditWithAccounts = _context.Installments.Where(x => x.Id == credit.Id).Include(x => x.BankAccount).Single();
                    credit.BankAccount.Status = "Block";
                }

            }
            creditWithBank.Add(credits, bankId);
            return View("Views/Client/Credit/Credit.cshtml", creditWithBank);
        }

        [HttpGet]
        public async Task<IActionResult> ApplyForCredit(int bankId)
        {
            ApplyCreditAndInstallment.BankId = bankId;
            return View("Views/Client/Credit/ApplyForCredit.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyForCredit(ApplyCreditAndInstallment apply)
        {
            string AccName = null;
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).SingleOrDefault();
                Client client = userWithClient.Clients.First();
                var clientWithAccount = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).SingleOrDefault();
                var clientWithInstallments = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Installments).SingleOrDefault();

                foreach (var acc in clientWithAccount.Accounts)
                {
                    if (apply.BankAccount == acc.Name)
                        if (acc.BankId == ApplyCreditAndInstallment.BankId)
                        {
                            if (acc.Status == "Freeze")
                            {
                                return View("Views/Client/FreezingAccount.cshtml");
                            }
                            if (acc.Status == "Block")
                            {
                                return View("Views/Client/BlockAccount.cshtml");
                            }
                            AccName = acc.Name;
                            apply.ClientId = client.Id;
                            apply.ClientName = userName;
                            apply.BalanceId = acc.Id;


                            CreateInstallment create = new() { Money = apply.Money, 
                                MoneyProcent = apply.MoneyProcent, Count= apply.Count, AccountId = (int)apply.BalanceId, 
                                ClientId = apply.ClientId, BankId = (int)ApplyCreditAndInstallment.BankId, ClientName = apply.ClientName };
                            _context.InstallmentCreation.Add(create);
                            _context.SaveChanges();
                        }
                }
                if (AccName == null)
                {
                    return View("Views/Client/Credit/ApplyForCredit.cshtml", apply);
                }

                return View("Views/Client/Installment/ApplicationForInstallment.cshtml");
            }
            else
            {
                return View("Views/Client/Credit/ApplyForCredit.cshtml", apply);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SumToCredit(int id)
        {
            InstallmentPlan installment = await _context.Installments.FindAsync(id);
            var installmentWithAccount = _context.Installments.Where(x => x.Id == installment.Id).Include(x => x.BankAccount).SingleOrDefault();
            
            if (installmentWithAccount.BankAccount.Status == "Freeze")
            {
                return View("Views/Client/FreezingAccount.cshtml");
            }

            if (installmentWithAccount.BankAccount.Status == "Block")
            {
                return View("Views/Client/BlockAccount.cshtml");
            }
            List<double> money = money = new() { installmentWithAccount.BankAccount.Money,
                installment.MinusMoney, 
                installmentWithAccount.NowMoney, id };

            return View("Views/Client/Credit/SumToCredit.cshtml", money);

        }


        [HttpPost]
        public async Task<IActionResult> SumToCredit(int id, string Money)
        {
            int current;
            string userName = HttpContext.User.Identity.Name;
            User user = await _userManager.FindByNameAsync(userName);
            var userWithClient = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Clients).Single();
            if (!Int32.TryParse(Money, out current))
            {
               return View("Views/Client/PutSumError.cshtml");

            }

            InstallmentPlan credit = await _context.Installments.FindAsync(id);
            var creditWithBank = _context.Installments.Where(x => x.Id == credit.Id).Include(x => x.Bank).SingleOrDefault();
            var creditWithAccount = _context.Installments.Where(x => x.Id == credit.Id).Include(x => x.BankAccount).SingleOrDefault();
            var account = creditWithAccount.BankAccount;
            var accountWithCredits = _context.BankAccounts.Where(x => x.Id == account.Id).Include(x => x.Installments).SingleOrDefault();

            var creditWithClient = _context.Installments.Where(x => x.Id == credit.Id).Include(x => x.Client).SingleOrDefault();
            var client = creditWithAccount.Client;
            var clientWithCredits = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Installments).SingleOrDefault();

            var bankWithInstallments = _context.Banks.Where(x => x.Id == creditWithBank.BankId).Include(x => x.Installments).SingleOrDefault();
            double temp = credit.NowMoney - credit.MinusMoney;

            if (temp == current)
            {
                if (account.Money < current)
                     return View("Views/Client/PutSumError.cshtml");
                else
                {
                    credit.Status = false;
                    account.Money -= current;
                    credit.NowMoney += current;
                    _context.SaveChanges();
                }
            }
            else if (temp > current)
            {
                if (account.Money > current)
                {
                    account.Money -= current;
                    credit.NowMoney += current;
                    _context.SaveChanges();
                }
                else
                    return View("Views/Client/PutSumError.cshtml");
            }
            else if (temp < current)
            {
                return View("Views/Client/PutSumError.cshtml");
            }

            MoneyStatistic money = new() { Operation = "PayCredit", Status = true, 
                Money = current, ClientId = userWithClient.Clients.First().Id,
                BankId = creditWithBank.BankId, 
                InstallmentId = credit.Id };
            _context.MoneyStatistics.Add(money);
            _context.SaveChanges();
            return View("Views/Client/Index.cshtml");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
