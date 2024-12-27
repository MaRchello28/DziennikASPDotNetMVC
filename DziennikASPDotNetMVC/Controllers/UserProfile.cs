using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DziennikASPDotNetMVC.Controllers
{
    [Authorize]
    public class UserProfile : Controller
    {
        private readonly MyDbContext db;
        public UserProfile(MyDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");

            int id;

            if(int.TryParse(userId, out id))
            {
                User user = db.User.FirstOrDefault(u => u.userId == id);
                return View(user);
            }
            else
            {
                return View();
            }
        }

        public string UserRole(int userId)
        {
            User user = db.User.FirstOrDefault(u => u.userId == userId);
            if (user == null)
            {
                return "";
            }
            else
            {
                return user.type;
            }
        }
    }
}
