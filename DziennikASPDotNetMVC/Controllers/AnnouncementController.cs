using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    public class AnnouncementController : Controller
    {
        public MyDbContext db;
        public AnnouncementController(MyDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            Console.WriteLine("Byłem tu");
            var announcements = db.Announcements.OrderByDescending(a => a.whenUpload).ToList();
            return View(announcements);
        }

        public IActionResult GetImage(int id)
        {
            var announcement = db.Announcements.FirstOrDefault(a => a.id == id);
            if (announcement != null && announcement.image != null)
            {
                return File(announcement.image, "image/png");
            }

            var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "DefaultAnnouncement.png");
            var imageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
            return File(imageBytes, "image/png");
        }

        
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public async Task<IActionResult> Create(Announcement announcement, IFormFile image)
        {
            var userId = HttpContext.Session.GetString("UserId");
            int.TryParse(userId, out int id);
            ModelState.Remove("image");
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        announcement.image = memoryStream.ToArray();
                    }
                }
                else
                {
                    announcement.image = null;
                }

                announcement.whenUpload = DateTime.Now;
                announcement.teacherId = id;
                db.Add(announcement);
                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }
    }
}
