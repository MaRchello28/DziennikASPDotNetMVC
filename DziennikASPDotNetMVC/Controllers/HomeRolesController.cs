using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DziennikASPDotNetMVC.Controllers
{
    public class HomeRolesController : Controller
    {
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminView()
        {
            //To sprawdza czy sesja działa
            var username = HttpContext.Session.GetString("UserId");
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

        [Authorize(Policy = "StudentOnly")]
        public IActionResult StudentView()
        {
            return View();
        }

        [Authorize(Policy = "ParentOnly")]
        public IActionResult ParentView()
        {
            return View();
        }

        [Authorize(Policy = "TeacherOnly")]
        public IActionResult TeacherView()
        {
            return View();
        }
    }
}
