using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace DziennikASPDotNetMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext db;

        public HomeController(ILogger<HomeController> logger, MyDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            var latestAnnouncements = db.Announcements
                .OrderByDescending(a => a.whenUpload)
                .Take(2)
                .ToList();

            ViewData["LatestAnnouncements"] = latestAnnouncements;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userRole = HttpContext.Session.GetString("UserRole");
                var userId = HttpContext.Session.GetString("UserId");

                if (string.IsNullOrEmpty(userRole) && string.IsNullOrEmpty(userId))
                {
                    userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                    if (!string.IsNullOrEmpty(userRole))
                    {
                        HttpContext.Session.SetString("UserRole", userRole);
                    }

                    if (!string.IsNullOrEmpty(userId))
                    {
                        HttpContext.Session.SetString("UserId", userId);
                    }
                }

                // Przekierowanie u¿ytkownika na odpowiedni¹ stronê
                switch (userRole?.ToLower())
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
                    HttpContext.Session.SetString("UserId", user.userId.ToString());
                    HttpContext.Session.SetString("UserRole", user.type);

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.userId.ToString()),
                new Claim(ClaimTypes.Role, user.type)
            };

                    var identity = new ClaimsIdentity(claims, "SessionAuthentication");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync("SessionAuthentication", principal);

                    ViewData["UserLogged"] = true;

                    _logger.LogInformation($"U¿ytkownik {user.login} zalogowa³ siê jako {user.type}.");

                    if (string.IsNullOrWhiteSpace(user.email))
                    {
                        ViewData["EmailRequired"] = true;
                        return RedirectToAction("EnterEmail"); // Zmiana na przekierowanie do EnterEmail
                    }

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

        // Logout action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();

            ViewData["UserLogged"] = false;

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EnterEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                ModelState.AddModelError("email", "Proszê podaæ prawid³owy adres email.");
                return View("EnterEmail");
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var user = db.User.FirstOrDefault(u => u.userId.ToString() == userId);
                if (user != null)
                {
                    user.email = email;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login", "UserProfile");
        }
    }
}