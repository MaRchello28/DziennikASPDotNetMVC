using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace DziennikASPDotNetMVC.Controllers
{
    public class InquiryController : Controller
    {
        private readonly MyDbContext db;
        public InquiryController(MyDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserId");
            int.TryParse(username, out int id);
            var user = db.User.FirstOrDefault(u => u.userId == id);
            var inqu = db.Inquiryes.Where(m => m.from == user.login || m.to == user.login).ToList();
            return View(inqu);
        }

        public ActionResult Details(int id)
        {
            var mail = db.Inquiryes.Find(id);
            if (mail == null) return RedirectToAction("Index");
            return View(mail);
        }

        // GET: Mail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("subject, body, to")] Inquiry mail)
        {
            ModelState.Remove("from");
            if (ModelState.IsValid)
            {
                var username = HttpContext.Session.GetString("UserId");
                int.TryParse(username, out int id);
                var user = db.User.FirstOrDefault(u => u.userId == id);
                mail.from = user.login;
                mail.send = DateTime.Now;
                db.Inquiryes.Add(mail);
                db.SaveChanges();
                var userToSendMail = db.User.FirstOrDefault(u => u.login == mail.to);

                await SendEmailAsync(userToSendMail.email, "Nowe zapytanie", "Masz nowe zapytanie w dzienniku.");

                return RedirectToAction("Index");
            }

            return View(mail);
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mateoprokop@gmail.com", "zhnk exxu vrwt njfq"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("mateoprokop@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
