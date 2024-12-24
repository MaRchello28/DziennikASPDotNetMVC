using Microsoft.AspNetCore.Mvc;

namespace DziennikASPDotNetMVC.Controllers
{
    public class HomeRolesController : Controller
    {
        public IActionResult AdminView()
        {
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
