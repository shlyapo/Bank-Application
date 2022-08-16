using BankApp.Data;
using BankApp.Models.Entity;
using BankApp.Models.Entity.SalaryModel;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Operator")]
    public class OperatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        UserManager<User> _userManager;
        readonly SignInManager<User> _signManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;

        public OperatorController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }


        public IActionResult ChooseCompany()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Operators).Single();
            Operator manager = userWithManager.Operators.First();

            var managerWithBank = _context.Operators.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Bank bank = _context.Banks.Find(managerWithBank.BankId);
            var bankingWithCompany = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.Companys).Single();
            return View("Views/Manager/ChooseCompany.cshtml", bankingWithCompany.Companys.ToList());
        }
        public IActionResult WatchClientApplication(int CompanyId)
        {
            Company comp = _context.Companies.Find(CompanyId);
            var compWithBank = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.Bank).Single();
            var compWithClentApp = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.ClientSalaries).Single();
            Bank bank = _context.Banks.Find(compWithBank.BankId);
            var bankWithSalaryApplication = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.SalaryProjects).Single();

            ClientApplicationForSimular checking = new();

            foreach (var salaryapplication in bankWithSalaryApplication.SalaryProjects.ToList())
            {
                foreach (var clientSalarys in compWithClentApp.ClientSalaries.ToList())
                {
                    if (salaryapplication.ClientId == clientSalarys.ClientId && salaryapplication.SalaryId == clientSalarys.SalaryId && clientSalarys.CompanyId == comp.Id)
                    {
                        salaryapplication.Simular = "Has documents";
                        Client client = _context.Clients.Find(salaryapplication.ClientId);
                        var clientWithUser = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                        checking.UserSalary.Add(clientWithUser.User, _context.Salarys.Find(salaryapplication.SalaryId));
                        checking.Applications.Add(salaryapplication);
                        _context.SaveChanges();
                    }
                }

            }
            return View("Views/Operator/WatchClientApplication.cshtml", checking);
        }

        public IActionResult ConfirmSalaryProject(int SalaryProjectId)
        {
            SalaryProject application = _context.SalaryProjects.Find(SalaryProjectId);
            Bank bank = _context.Banks.Find(application.BankId);
            Salary salary = _context.Salarys.Find(application.SalaryId);
            var salaryWithCompany = _context.Salarys.Where(x => x.Id == salary.Id).Include(x => x.Company).Single();
            Company comp = salaryWithCompany.Company;
            var compWithClientSalary = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.ClientSalaries).Single();
            Client client = _context.Clients.Find(application.ClientId);
            var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            foreach (var acc in clientWithAccounts.Accounts.ToList())
            {
                if (acc.Name == salary.Profession)
                {
                    acc.Salary = true;
                    client.Profession = salary.Profession;
                    client.PayForProfession = salary.Money;
                    client.CompanyId = comp.Id;
                    bank.SalaryProjects.Remove(application);
                    foreach (var sal in compWithClientSalary.ClientSalaries.ToList())
                    {
                        if (sal.ClientId == application.ClientId)
                        {
                            comp.ClientSalaries.Remove(sal);
                            _context.ClientSalaries.Remove(sal);
                        }
                    }
                    _context.SalaryProjects.Remove(application);

                    _context.SaveChanges();

                    return View("Views/Operator/ConfirmSalaryProject.cshtml");
                }
            }
            BankAccount accforpayment = new() { Client = client, Salary = true, Bank = bank, Name = salary.Profession, Status = "Usual", Money = 0 };
            client.Profession = salary.Profession;
            client.PayForProfession = salary.Money;
            client.CompanyId = comp.Id;
            bank.Accounts.Add(accforpayment);
            bank.SalaryProjects.Remove(application);
            foreach (var sal in compWithClientSalary.ClientSalaries.ToList())
            {
                if (sal.ClientId == application.ClientId)
                {
                    comp.ClientSalaries.Remove(sal);
                    _context.ClientSalaries.Remove(sal);
                }

            }
            _context.SalaryProjects.Remove(application);
            _context.SaveChanges();

            return View("Views/Operator/ConfirmSalaryProject.cshtml");

        }
        /*
        public IActionResult ChooseBankApp()
        {
            return View(_context.Banks.ToList());
        }

        public IActionResult ChooseCompany(int BankId)
        {
            Bank bank = _context.Banks.Find(BankId);
            var bankingWithComp = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.Companys).SingleOrDefault();
            return View(bankingWithComp.Companys.ToList());
        }
        public IActionResult WatchClientApplication(int CompId)
        {
            Company comp = _context.Companies.Find(CompId);
            var compWithBank = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.Bank).Single();
            var enterpriseWithClentApplicatiob = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.ClientSalaries).Single();
            Bank bank = _context.Banks.Find(compWithBank.BankId);
            var bnkWithSalaryApplication = _context.Banks.Where(x => x.Id == bank.Id).Include(x => x.SalaryProjects).Single();
            ClientApplicationForSimular checking = new();

            foreach (var salaryapplication in bnkWithSalaryApplication.SalaryProjects.ToList())
            {
                foreach (var clientSalarys in enterpriseWithClentApplicatiob.ClientSalaries.ToList())
                {
                    if (salaryapplication.ClientId == clientSalarys.ClientId && salaryapplication.SalaryId == clientSalarys.SalaryId && clientSalarys.CompanyId == comp.Id)
                    {
                        salaryapplication.Simular = "Has documents";
                        Client client = _context.Clients.Find(salaryapplication.ClientId);
                        var clientWithUser = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                        checking.UserSalary.Add(clientWithUser.User, _context.Salarys.Find(salaryapplication.SalaryId));
                        checking.Applications.Add(salaryapplication);
                        _context.SaveChanges();
                    }
                }

            }
            return View(checking);
        }

        public IActionResult ConfirmSalaryProject(int SalaryProjectApplicationId)
        {
            SalaryProject application = _context.SalaryProjects.Find(SalaryProjectApplicationId);
            Bank bank = _context.Banks.Find(application.BankId);
            Salary salary = _context.Salarys.Find(application.SalaryId);
            var salaryWithComp = _context.Salarys.Where(x => x.Id == salary.Id).Include(x => x.Company).Single();
            Company comp = salaryWithComp.Company;
            var compWithClientSalary = _context.Companies.Where(x => x.Id == comp.Id).Include(x => x.ClientSalaries).Single();
            Client client = _context.Clients.Find(application.ClientId);
            var clientWithAccounts = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.Accounts).Single();
            foreach (var acc in clientWithAccounts.Accounts.ToList())
            {
                if (acc.Name == salary.Profession)
                {
                    acc.Salary = true;
                    client.Profession = salary.Profession;
                    client.PayForProfession = salary.Money;
                    client.CompanyId = comp.Id;
                    bank.SalaryProjects.Remove(application);
                    foreach (var sal in compWithClientSalary.ClientSalaries.ToList())
                    {
                        if (sal.ClientId == application.ClientId)
                        {
                            comp.ClientSalaries.Remove(sal);
                            _context.ClientSalaries.Remove(sal);
                        }

                    }
                    _context.SalaryProjects.Remove(application);


                    _context.SaveChanges();

                    return View();
                }
            }
            BankAccount accforpayment = new() { Client = client, Salary = true, 
                Bank = bank, Name = salary.Profession, 
                Status = "Usual", Money = 0 };
            client.Profession = salary.Profession;
            client.PayForProfession = salary.Money;
            client.CompanyId = comp.Id;
            bank.Accounts.Add(accforpayment);
            bank.SalaryProjects.Remove(application);
            foreach (var sal in compWithClientSalary.ClientSalaries.ToList())
            {
                if (sal.ClientId == application.ClientId)
                {
                    comp.ClientSalaries.Remove(sal);
                    _context.ClientSalaries.Remove(sal);
                }

            }
            _context.SalaryProjects.Remove(application);
            _context.SaveChanges();

            return View();

        }*/
        
        public IActionResult Statistics()
        {
            string userName = HttpContext.User.Identity.Name;

            var userWithOperator = _context.Users.Where(x => x.UserName == userName).Include(x => x.Operators).Single();
            Operator operat = userWithOperator.Operators.First();

            var operatorWithBank = _context.Managers.Where(x => x.Id == operat.Id).Include(x => x.Bank).Single();
            Bank bnk = _context.Banks.Find(operatorWithBank.BankId);
            var bnkWithClients = _context.Banks.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
            Dictionary<Client, User> userClient = new();
            foreach (var client in bnkWithClients.Clients.ToList())
            {
                var clientWithUser = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                userClient.Add(client, clientWithUser.User);
            }

            return View("Views/Manager/Statistics.cshtml", userClient);
        }
        
        public IActionResult PersonalStatistics(int client)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Operators).Single();
            Operator operat = userWithManager.Operators.First();

            var managerWithBank = _context.Operators.Where(x => x.Id == operat.Id).Include(x => x.Bank).Single();
            Bank bnk = _context.Banks.Find(managerWithBank.BankId);
            MoneyStatisticsForClient.IndividualInformation = new();
            foreach (var el in _context.MoneyStatistics.ToList())
            {
                if (el.ClientId == client && el.BankId == bnk.Id)
                {
                    MoneyStatisticsForClient.IndividualInformation.Add(el);
                    if (el.Status == false)
                    {
                        MoneyStatisticsForClient.Status = false;
                    }
                }

            }
            MoneyStatisticsForClient.IndividualStatistic = new();
            if (MoneyStatisticsForClient.IndividualInformation.Count >= 2)
            {
                MoneyStatisticsForClient.IndividualStatistic.Push(MoneyStatisticsForClient.IndividualInformation[MoneyStatisticsForClient.IndividualInformation.Count - 2]);
                MoneyStatisticsForClient.IndividualStatistic.Push(MoneyStatisticsForClient.IndividualInformation[MoneyStatisticsForClient.IndividualInformation.Count - 1]);
            }

            if (MoneyStatisticsForClient.IndividualInformation.Count == 1)
            {
                MoneyStatisticsForClient.IndividualStatistic.Push(MoneyStatisticsForClient.IndividualInformation[MoneyStatisticsForClient.IndividualInformation.Count - 1]);
            }
            MoneyStatisticsForClient statistics = new();
            return View("Views/Manager/PersonalStatistics.cshtml", statistics);
        }

        public async Task<IActionResult> RejectPersonalStatistics(int MoneyStaticId)
        {
            MoneyStatistic statistic = await _context.MoneyStatistics.FindAsync(MoneyStaticId);
            MoneyStatistic statistic1 = MoneyStatisticsForClient.IndividualStatistic.Peek();


            if (statistic.Operation == "AddSumToAccount")
            {
                BankAccount account = await _context.BankAccounts.FindAsync(statistic.AccountId);
                account.Money -= statistic.Money;
                _context.SaveChanges();
            }
            if (statistic.Operation == "PutSumToCreditOrInstallment")
            {
                InstallmentPlan installment = await _context.Installments.FindAsync(statistic.InstallmentId);
                if (installment.Status == false)
                {
                    installment.Status = true;
                }
                InstallmentPlan inst = await _context.Installments.FindAsync(statistic.InstallmentId);
                var instWithAccount = _context.Installments.Where(x => x.Id == inst.Id).Include(x => x.BankAccount).Single();
                if (instWithAccount.BankAccount.Status == "Freeze")
                {
                    instWithAccount.BankAccount.Status = "Usual";
                }
                instWithAccount.BankAccount.Money += statistic.Money;
                inst.NowMoney -= statistic.Money;
            }
            if (statistic.Id != statistic1.Id)
            {
                MoneyStatisticsForClient.IndividualStatistic.Pop();
                if (statistic.Operation == "AddSumToAccount")
                {
                    BankAccount account = await _context.BankAccounts.FindAsync(statistic.AccountId);
                    account.Money -= statistic.Money;
                    _context.SaveChanges();
                }
                if (statistic.Operation == "PutSumToCreditOrInstallment")
                {
                    InstallmentPlan installment = await _context.Installments.FindAsync(statistic.InstallmentId);
                    if (installment.Status == false)
                    {
                        installment.Status = true;
                    }
                    InstallmentPlan inst = await _context.Installments.FindAsync(statistic.InstallmentId);
                    var instWithAccount = _context.Installments.Where(x => x.Id == inst.Id).Include(x => x.BankAccount).Single();
                    if (instWithAccount.BankAccount.Status == "Freeze")
                    {
                        instWithAccount.BankAccount.Status = "Usual";
                    }
                    instWithAccount.BankAccount.Money += statistic.Money;
                    inst.NowMoney -= statistic.Money;
                }


            }
            statistic.Status = false;
            _context.SaveChanges();

            return View("Index");
        }
    }
}
