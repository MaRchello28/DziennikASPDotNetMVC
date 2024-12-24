using Microsoft.AspNetCore.Mvc;

namespace DziennikASPDotNetMVC.Controllers
{
    public class HomeRolesController : Controller
    {
        public IActionResult AdminView()
        {
            var username = HttpContext.Session.GetString("Username");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userRole))
            {
                ViewData["Username"] = username;
                ViewData["UserRole"] = userRole;
            }
            else
            {
                ViewData["Message"] = "Brak danych w sesji.";
            }

            return View();
        }

        public IActionResult StudentView()
        {
            return View();
        }

        public IActionResult ParentView()
        {
            return View();
        }

        public IActionResult TeacherView()
        {
            return View();
        }
    }
}
