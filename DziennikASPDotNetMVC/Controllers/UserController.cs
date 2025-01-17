using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly MyDbContext db;
        public UserController(MyDbContext db)
        {
            this.db = db;
        }
        // GET: UserController
        public async Task<IActionResult> Index()
        {
            var users = await db.User.ToListAsync();

            return View(users);
        }

        // GET: UserController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await db.User.FirstOrDefaultAsync(u => u.userId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("name,surname,type,login,password")] User user)
        {

            if (user.type == "admin")
            {
                if (string.IsNullOrEmpty(user.login) || string.IsNullOrEmpty(user.password))
                {
                    ModelState.AddModelError("", "Login i hasło są wymagane dla użytkownika typu 'admin'.");
                    return View(user);
                }
            }
            else
            {
                user.login = (user.name.Substring(0, Math.Min(3, user.name.Length)) + user.surname).ToLower();
                user.password = GenerateRandomPassword(15);
                ModelState.Remove("login");
                ModelState.Remove("password");
            }

            if (ModelState.IsValid)
            {
                user.email = null;
                db.User.Add(user);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await db.User.FirstOrDefaultAsync(u => u.userId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userId,name,surname,login,password,type,email")] User user)
        {
            if (id != user.userId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(user);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!UserExists(user.userId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {

            var user = await db.User.FirstOrDefaultAsync(u => u.userId == id);

            if (user == null)
            {
                return NotFound(); 
            }

            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await db.User.FirstOrDefaultAsync(u => u.userId == id);

            if (user != null)
            {
                db.User.Remove(user);
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(int id)
        {
            return db.User.Any(e => e.userId == id);
        }
        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
