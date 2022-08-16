using BankApp.Data;
using BankApp.Models.Entity;
using BankApp.Models.Entity.SalaryModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankApp.ViewModels
{
    public class BankRepository
    {
        static UserManager<User> _userManager;
        static RoleManager<User> _roleManager;
        static ApplicationDbContext _context;
        static List<Bank> banks { get; set; } = new();
        public static async Task Initialize(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;



            if (await _context.Managers.FindAsync(1) == null)
            {
                User user = new User { Email = "manager@gmail.com", UserName = "manager", PhoneNumber = "56789765",
                    PassportSerier = "HB", PassportNum = 5678765, IdentificationNum = "45678976" };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Manager");
                Bank banking = await _context.Banks.FindAsync(1);
                Manager manager = new() { User = user, Bank = banking };
                var bnkWithManagers = _context.Banks.Where(x => x.Id == banking.Id).Include(x => x.Managers).Single();
                bnkWithManagers.Managers.Add(manager);
                _context.Managers.Add(manager);
                user.Managers.Add(manager);
                _context.SaveChanges();
            }

            if (await _context.Managers.FindAsync(2) == null)
            {
                User user = new User
                {
                    Email = "managerB@gmail.com",
                    UserName = "managerB",
                    PhoneNumber = "56789765",
                    PassportSerier = "HB",
                    PassportNum = 5678765,
                    IdentificationNum = "45678976"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Manager");
                Bank banking = await _context.Banks.FindAsync(2);
                Manager manager = new() { User = user, Bank = banking };
                var bnkWithManagers = _context.Banks.Where(x => x.Id == banking.Id).Include(x => x.Managers).Single();
                bnkWithManagers.Managers.Add(manager);
                _context.Managers.Add(manager);
                user.Managers.Add(manager);
                _context.SaveChanges();
            }
            if (await _context.Managers.FindAsync(3) == null)
            {
                User user = new User
                {
                    Email = "managerT@gmail.com",
                    UserName = "managerT",
                    PhoneNumber = "56789765",
                    PassportSerier = "HB",
                    PassportNum = 5678765,
                    IdentificationNum = "45678976"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Manager");
                Bank banking = await _context.Banks.FindAsync(3);
                Manager manager = new() { User = user, Bank = banking };
                var bnkWithManagers = _context.Banks.Where(x => x.Id == banking.Id).Include(x => x.Managers).Single();
                bnkWithManagers.Managers.Add(manager);
                _context.Managers.Add(manager);
                user.Managers.Add(manager);
                _context.SaveChanges();
            }

            if (await _context.Admins.FindAsync(1) == null)
            {
                User user = new User
                {
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    PhoneNumber = "56789765",
                    PassportSerier = "MK",
                    PassportNum = 5678765,
                    IdentificationNum = "45678976"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Admin");
                //Bank banking = await _context.Banks.FindAsync(1);
                Admin admin = new() { User = user};
                //var bnkWithManagers = _context.Banks.Where(x => x.Id == banking.Id).Include(x => x.Managers).Single();
                //bnkWithManagers.Managers.Add(manager);
                _context.Admins.Add(admin);
                user.Admins.Add(admin);
                _context.SaveChanges();
            }


            if (await _context.Operators.FindAsync(1) == null)
            {
                User user = new User { Email = "operator@gmail.com", UserName = "operator", PhoneNumber = "5678765", 
                    PassportSerier = "LB", PassportNum = 45678765, IdentificationNum = "67TGVUYGUI" };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Operator");
                Bank banking = await _context.Banks.FindAsync(1);
                Operator op = new() { User = user, Bank = banking };
                banking.Operators.Add(op);
                _context.Operators.Add(op);

                user.Operators.Add(op);
                _context.SaveChanges();

            }

            if (await _context.Operators.FindAsync(2) == null)
            {
                User user = new User
                {
                    Email = "operatorB@gmail.com",
                    UserName = "operatorB",
                    PhoneNumber = "5678765",
                    PassportSerier = "LB",
                    PassportNum = 45678765,
                    IdentificationNum = "67TGVUYGUI"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Operator");
                Bank banking = await _context.Banks.FindAsync(2);
                Operator op = new() { User = user, Bank = banking };
                banking.Operators.Add(op);
                _context.Operators.Add(op);

                user.Operators.Add(op);
                _context.SaveChanges();

            }

            if (await _context.Operators.FindAsync(3) == null)
            {
                User user = new User
                {
                    Email = "operatorT@gmail.com",
                    UserName = "operatorT",
                    PhoneNumber = "5678765",
                    PassportSerier = "LB",
                    PassportNum = 45678765,
                    IdentificationNum = "67TGVUYGUI"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Operator");
                Bank banking = await _context.Banks.FindAsync(3);
                Operator op = new() { User = user, Bank = banking };
                banking.Operators.Add(op);
                _context.Operators.Add(op);

                user.Operators.Add(op);
                _context.SaveChanges();

            }


            if (await _context.Companies.FindAsync(1) == null)
            {
                await _context.Companies.AddAsync(new Company() { TYP = "Shlyapo", YrName = "Liza", 
                    BIK = "4865864", YrAdress = "chlhvlgcvyh", 
                    Bank = await _context.Banks.FindAsync(1), Salarys= (ICollection<Salary>)await _context.Salarys.FindAsync(1) });
                _context.SaveChanges();

            }

            if (await _context.Companies.FindAsync(2) == null)
            {
                await _context.Companies.AddAsync(new Company() { TYP = "HEEE", YrName = "IDIDI", 
                     BIK = "43E636", YrAdress = "DFGHKJL", 
                    Bank = await _context.Banks.FindAsync(2),
                    Salarys = (ICollection<Salary>)await _context.Salarys.FindAsync(2)
                });
                _context.SaveChanges();

            }
            if (await _context.Companies.FindAsync(3) == null)
            {
                await _context.Companies.AddAsync(new Company()
                {
                    TYP = "HEEE",
                    YrName = "Foo",
                    BIK = "43E636",
                    YrAdress = "DFGHKJL",
                    Bank = await _context.Banks.FindAsync(3),
                    Salarys = (ICollection<Salary>)await _context.Salarys.FindAsync(3)
                });
                _context.SaveChanges();

            }

            if (await _context.Specialists.FindAsync(1) == null)
            {
                User user = new User { Email = "spec@gmail.com", UserName = "spec", PhoneNumber = "5678876754", 
                    PassportSerier = "BM", PassportNum= 56789876, IdentificationNum = "678976R57TFU" };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Specialist");
                Bank banking = await _context.Banks.FindAsync(1);
                Specialist ent = new() { User = user, Bank = banking, Company = await _context.Companies.FindAsync(1) };
                _context.Specialists.Add(ent);
                banking.Specialists.Add(ent);
                _context.SaveChanges();
            }

            if (await _context.Specialists.FindAsync(2) == null)
            {
                User user = new User
                {
                    Email = "specB@gmail.com",
                    UserName = "specB",
                    PhoneNumber = "5678876754",
                    PassportSerier = "BM",
                    PassportNum = 56789876,
                    IdentificationNum = "678976R57TFU"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Specialist");
                Bank banking = await _context.Banks.FindAsync(2);
                Specialist ent = new() { User = user, Bank = banking, Company = await _context.Companies.FindAsync(2) };
                _context.Specialists.Add(ent);
                banking.Specialists.Add(ent);
                _context.SaveChanges();
            }

            if (await _context.Specialists.FindAsync(3) == null)
            {
                User user = new User
                {
                    Email = "specT@gmail.com",
                    UserName = "specT",
                    PhoneNumber = "5678876754",
                    PassportSerier = "BM",
                    PassportNum = 56789876,
                    IdentificationNum = "678976R57TFU"
                };

                var result = await _userManager.CreateAsync(user, "123@Qa");

                await _userManager.AddToRoleAsync(user, "Specialist");
                Bank banking = await _context.Banks.FindAsync(3);
                Specialist ent = new() { User = user, Bank = banking, Company = await _context.Companies.FindAsync(3) };
                _context.Specialists.Add(ent);
                banking.Specialists.Add(ent);
                _context.SaveChanges();
            }

            if (await _context.Salarys.FindAsync(1) == null)
            {

                Company comp = await _context.Companies.FindAsync(1);
                Salary programmer = new() { Profession = "Director", Money = 100000, Company = comp};
                await _context.Salarys.AddAsync(programmer);
                comp.Salarys.Add(programmer);

                Salary manager = new() { Profession = "Manager", Money = 77575, Company = comp };
                await _context.Salarys.AddAsync(manager);
                comp.Salarys.Add(manager);


                _context.SaveChanges();
            }

            if (await _context.Salarys.FindAsync(2) == null)
            {

                Company comp = await _context.Companies.FindAsync(2);
                Salary programmer = new() { Profession = "Director", Money = 100000, Company = comp };
                await _context.Salarys.AddAsync(programmer);
                comp.Salarys.Add(programmer);
                Company comp1 = await _context.Companies.FindAsync(2);
                Salary manager = new() { Profession = "Manager", Money = 77575, Company = comp1 };
                await _context.Salarys.AddAsync(manager);
                comp.Salarys.Add(manager);


                _context.SaveChanges();
            }

            if (await _context.Salarys.FindAsync(3) == null)
            {

                Company comp = await _context.Companies.FindAsync(3);
                Salary programmer = new() { Profession = "Director", Money = 100000, Company = comp };
                await _context.Salarys.AddAsync(programmer);
                comp.Salarys.Add(programmer);
                Company comp1 = await _context.Companies.FindAsync(3);
                Salary manager = new() { Profession = "Manager", Money = 77575, Company = comp1 };
                await _context.Salarys.AddAsync(manager);
                comp.Salarys.Add(manager);


                _context.SaveChanges();
            }

            if (await _context.Banks.FindAsync(1) == null)
            {
                await _context.Banks.AddAsync(new Bank() { Name = "BelarusBank", Companys= (ICollection<Company>)await _context.Companies.FindAsync(1) });
            }
            if (await _context.Banks.FindAsync(2) == null)
            {
                await _context.Banks.AddAsync(new Bank() { Name = "BelinvestBank", Companys = (ICollection<Company>)await _context.Companies.FindAsync(2) });
            }
            if (await _context.Banks.FindAsync(3) == null)
            {
                await _context.Banks.AddAsync(new Bank() { Name = "Tehnobank", Companys = (ICollection<Company>)await _context.Companies.FindAsync(3) });
            }
            _context.SaveChanges();

            _context.SaveChanges();


            _context.SaveChanges();


            _context.SaveChanges();
        }

        public static Bank? FindByName(ApplicationDbContext context, string name)
        {
            foreach (var bnk in context.Banks)
            {
                if (bnk.Name == name)
                {
                    return bnk;
                }
            }
            return null;

        }
    }
}
