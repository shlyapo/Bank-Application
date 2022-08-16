using BankApp.Data;
using BankApp.Models.Entity;
using BankApp.Models.Entity.SalaryModel;
using BankApp.Models.InstallmentOrCreditModel;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;




        public ManagerController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }

        public IActionResult ClientRegistr()
        {

            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Managers.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Bank bnk = _context.Banks.Find(managerWithBank.BankId);

            List<UserCreation> creation = _context.UserCreations.Where(x => x.bankId == bnk.Id).ToList();
            return View(creation);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int clientId, int bankId)
        {
            Client client = _context.Clients.Find(clientId);
            Bank bank = _context.Banks.Find(bankId);
            client.Banks.Add(bank);
            bank.Clients.Add(client);
            foreach (var reg in _context.UserCreations.ToList())
            {
                if (reg.bankId == bankId && reg.clId == clientId)
                {
                    _context.UserCreations.Remove(reg);
                }
            }
            _context.SaveChanges();

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int clientId, int bankId)
        {
            foreach (var reg in _context.UserCreations.ToList())
            {
                if (reg.bankId == bankId && reg.clId == clientId)
                {
                    _context.UserCreations.Remove(reg);
                }
            }
            _context.SaveChanges();

            return View("Index");

        }
        public IActionResult ChooseCompany()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Managers.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
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
        
        public IActionResult Statistics()
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Managers.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
            Bank bnk = _context.Banks.Find(managerWithBank.BankId);
            var bnkWithClients = _context.Banks.Where(x => x.Id == bnk.Id).Include(x => x.Clients).Single();
            Dictionary<Client, User> userClient = new();
            foreach (var client in bnkWithClients.Clients.ToList())
            {
                var clientWithUser = _context.Clients.Where(x => x.Id == client.Id).Include(x => x.User).Single();
                userClient.Add(client, clientWithUser.User);
            }

            return View(userClient);
        }
        
        public IActionResult PersonalStatistics(int client)
        {
            string userName = HttpContext.User.Identity.Name;
            var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
            Manager manager = userWithManager.Managers.First();

            var managerWithBank = _context.Managers.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
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
            return View(statistics);
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
    //[Authorize(Roles = "Manager","Operator")]


    public IActionResult CreditRegistr()
    {
        string userName = HttpContext.User.Identity.Name;
        var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
        Manager manager = userWithManager.Managers.First();

        var managerWithBank = _context.Managers.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
        Bank bnk = _context.Banks.Find(managerWithBank.BankId);

        List<CreateInstallment> creation = _context.InstallmentCreation.Where(x => x.BankId == bnk.Id).Where(x => x.MoneyProcent != null).ToList();
        return View(creation);
    }

    public IActionResult InstallmentRegistr()
    {
        string userName = HttpContext.User.Identity.Name;
        var userWithManager = _context.Users.Where(x => x.UserName == userName).Include(x => x.Managers).Single();
        Manager manager = userWithManager.Managers.First();

        var managerWithBank = _context.Managers.Where(x => x.Id == manager.Id).Include(x => x.Bank).Single();
        Bank bnk = _context.Banks.Find(managerWithBank.BankId);

        List<CreateInstallment> creation = _context.InstallmentCreation.Where(x => x.BankId == bnk.Id).Where(x => x.MoneyProcent == null).ToList();
        return View(creation);
    }

    [HttpPost]
    public IActionResult RejectCreditOrInstallment(int Id)
    {
        CreateInstallment creation = _context.InstallmentCreation.Find(Id);

        _context.InstallmentCreation.Remove(creation);
        _context.SaveChanges();
        if (creation.MoneyProcent != null)
        {
            List<CreateInstallment> creations = new();
            foreach (var el in _context.InstallmentCreation.ToList())
            {
                if (el.BankId == creation.BankId && el.MoneyProcent != null)
                {
                    creations.Add(el);
                }
            }
            return View("CreditRegistr", creations);

        }
        if (creation.MoneyProcent == null)
        {
            List<CreateInstallment> creations = new();
            foreach (var el in _context.InstallmentCreation.ToList())
            {
                if (el.BankId == creation.BankId && el.MoneyProcent == null)
                {
                    creations.Add(el);
                }
            }
            return View("InstallmentRegistr", creations);

        }
        return View("Index");

    }

    [HttpPost]
    public async Task<IActionResult> ConfirmCredit(int Id)
    {
        CreateInstallment creation = _context.InstallmentCreation.Find(Id);

            Credit credit = new Credit();
            //credit.Id = Id;
            credit.MonthProcent = (int)creation.MoneyProcent;
            credit.OpenTime = DateTime.Now;
            credit.CloseTime = DateTime.Now.AddMonths(creation.Count);
            credit.MinusMoney = (int)(creation.Count * creation.MoneyProcent * 0.01 * creation.Money + creation.Money);
            credit.NowMoney = 0;
            credit.Count = creation.Count;
            credit.Name = "Credit";
        _context.Installments.Add(credit);
       // _context.SaveChanges();
            Client client = await _context.Clients.FindAsync(creation.ClientId);
        client.Installments.Add(credit);

        BankAccount account = await _context.BankAccounts.FindAsync(creation.AccountId);
        account.Installments.Add(credit);

        Bank bnk = await _context.Banks.FindAsync(creation.BankId);
        bnk.Installments.Add(credit);
            //credit.BankId = creation.BankId;
        account.Money += creation.Money;
        //_context.InstallmentCreation.Remove(creation);
        _context.SaveChanges();
        List<CreateInstallment> creations = new();
        foreach (var el in _context.InstallmentCreation.ToList())
        {
            if (el.BankId == creation.BankId && el.MoneyProcent != null)
            {
                creations.Add(el);
            }
        }

        return View("InstallmentRegistr", creations);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmInstallment(int Id)
    {
        CreateInstallment creation = _context.InstallmentCreation.Find(Id);

        InstallmentPlan installment = new() { OpenTime = DateTime.Now, CloseTime = DateTime.Now.AddMonths(creation.Count), 
            MinusMoney = creation.Money, NowMoney = 0, Count = creation.Count, Name="Installment"};
        _context.Installments.Add(installment);
            //_context.SaveChanges();
        Client client = await _context.Clients.FindAsync(creation.ClientId);
        client.Installments.Add(installment);

        BankAccount account = await _context.BankAccounts.FindAsync(creation.AccountId);
        account.Installments.Add(installment);

        Bank bnk = await _context.Banks.FindAsync(creation.BankId);
        bnk.Installments.Add(installment);

        account.Money += creation.Money;
        //_context.InstallmentCreation.Remove(creation);
        _context.SaveChanges();
        List<CreateInstallment> creations = new();
        foreach (var el in _context.InstallmentCreation.ToList())
        {
            if (el.BankId == creation.BankId && el.MoneyProcent == null)
            {
                creations.Add(el);
            }
        }

        return View("InstallmentRegistr", creations);
    }

}
}
