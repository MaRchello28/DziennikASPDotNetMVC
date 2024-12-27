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

        [HttpPost]
        public IActionResult EditField(string fieldName, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                TempData["Error"] = "Pole nie może być puste.";
                return RedirectToAction("Index");
            }
            var user = db.User.FirstOrDefault(u => u.userId == int.Parse(HttpContext.Session.GetString("UserId")));

            if (user == null)
            {
                TempData["Error"] = "Nie znaleziono użytkownika.";
                return RedirectToAction("Index");
            }

            switch (fieldName)
            {
                case "name":
                    user.name = newValue;
                    break;
                case "surname":
                    user.surname = newValue;
                    break;
                case "login":
                    user.login = newValue;
                    break;
                case "password":
                    if (newValue.Length < 6)
                    {
                        TempData["Error"] = "Hasło musi mieć co najmniej 6 znaków.";
                        return RedirectToAction("Index");
                    }
                    user.password = newValue;
                    break;
            }

            db.Update(user);
            db.SaveChanges();
            TempData["Success"] = "Dane zostały pomyślnie zaktualizowane.";
            return RedirectToAction("Index");
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
