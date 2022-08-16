using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using BankApp.Data;

namespace BankApp.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {/*
            string userName = HttpContext.User.Identity.Name;
            if (_context.Managers.Any(u => u.User!.UserName == userName) != null)
            {
                return RedirectToAction("Index", "Manager");
            }
            else if (_context.Operators.Any(u => u.User!.UserName == userName) != null)
            {
                return RedirectToAction("Index", "Operator");
            }
            else if (_context.Admins.Any(u => u.User!.UserName == userName) != null)
            {
                return RedirectToAction("Index", "Roles");
            }
            else if (_context.Clients.Any(u => u.User!.UserName == userName) != null)
            {
                return RedirectToAction("Index", "Client");
            }*/
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       /* public IActionResult Role()
        {
            string userName = HttpContext.User.Identity.Name;
            if (_context.Managers.Any(u => u.User!.UserName == userName) != null)
            {
                return View("Views/Manager/Index.cshtml", userName);
                
            }
            else if (_context.Operators.Any(u => u.User!.UserName == userName) != null)
            {
                return View("Views/Operator/Index.cshtml", userName);
            }
            else if (_context.Admins.Any(u => u.User!.UserName == userName) != null)
            {
                return View("Views/Roles/Index.cshtml", userName);
            }
            else if (_context.Clients.Any(u => u.User!.UserName == userName) != null)
            {
                return View("Views/Client/Index.cshtml", userName);
            }
            
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}