using Microsoft.AspNetCore.Mvc;
using BankApp.ViewModels;
using BankApp.Models.Entity;
using Microsoft.AspNetCore.Identity;
using BankApp.Data;

namespace BankApp.Controllers
{
    public class AccountController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Register(string userRole)
        {
            await Initializer.InitializeAsync(_userManager, _roleManager, _signInManager);
            await BankRepository.Initialize(_context, _userManager);
            await SendSalary.Initialize(_userManager, _context);
            return View("RegistrationUser");
        }
        /*
        [HttpGet]
        public IActionResult Register()
        {
            //BankRepository.CreateRole(_userManager, _signInManager, _roleManager);
            return View();
        }*/
        

        [HttpPost]
        public async Task<IActionResult> RegistrationUser(RegistrationUser model)
        {
            //User user = new User { Email = model.Email, UserName = model.UserName };
            //добавляем пользователя
            //var userWithClient = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Clients).SingleOrDefault();
            //Client cl = userWithClient.Clients.ToList()[0];
           // _context.SaveChanges();
            //var result = await _userManager.CreateAsync(user, model.Password);
           // await _userManager.AddToRoleAsync(user, "Client");
            ///user.Clients.Add(cl);

            if (ModelState.IsValid)
            {
                //User user = new User { Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                //var res = await _userManager.CreateAsync(user, model.Password);
                User user = new User { Email = model.Email, UserName = model.UserName, PhoneNumber = model.Number, PassportSerier = model.PassportSerier, PassportNum = model.PassportNum, IdentificationNum = model.IdentificationNum };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "Client");
                    Client client = new Client() { User = user };
                    client.User = user;
                    user.Clients.Add(client);
                    _context.Clients.Add(client);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Client");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        public async Task<IActionResult> ShowAllUsers()
        {
            return View(_userManager.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.NameUser, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}