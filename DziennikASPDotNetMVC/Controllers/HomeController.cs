using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

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
                    if (user.type == "admin")
                    {
                        return RedirectToAction("AdminView", "HomeRoles");
                    }
                    else if (user.type == "student")
                    {
                        return RedirectToAction("StudentView", "HomeRoles");
                    }
                    else if (user.type == "parent")
                    {
                        return RedirectToAction("ParentView", "HomeRoles");
                    }
                    else if (user.type == "teacher")
                    {
                        return RedirectToAction("TeacherView", "HomeRoles");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nieprawid³owa rola u¿ytkownika.");
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