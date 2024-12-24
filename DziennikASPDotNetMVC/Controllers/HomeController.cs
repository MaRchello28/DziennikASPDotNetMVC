using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DziennikASPDotNetMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        MyDbContext db;

        public HomeController(ILogger<HomeController> logger, MyDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.User
                    .FirstOrDefault(u => u.login == model.Username && u.password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("Username", user.login);
                    HttpContext.Session.SetString("UserRole", user.type);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.login),
                        new Claim(ClaimTypes.Role, user.type)
                    };

                    var identity = new ClaimsIdentity(claims, "SessionAuthentication");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync("SessionAuthentication", principal);

                    switch (user.type.ToLower())
                    {
                        case "admin":
                            return RedirectToAction("AdminView", "HomeRoles");
                        case "student":
                            return RedirectToAction("StudentView", "HomeRoles");
                        case "parent":
                            return RedirectToAction("ParentView", "HomeRoles");
                        case "teacher":
                            return RedirectToAction("TeacherView", "HomeRoles");
                        default:
                            ModelState.AddModelError("", "Nieprawid³owa rola u¿ytkownika.");
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nieprawid³owy login lub has³o.");
                }
            }
            return View(model);
        }
    }
}