using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DziennikASPDotNetMVC.Controllers
{
    public class MailController : Controller
    {
        private readonly MyDbContext db;
        public MailController(MyDbContext db)
        {
            this.db = db;
        }

        // GET: Mail
        public ActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserId");
            int.TryParse(username, out int id);
            var user = db.User.FirstOrDefault(u => u.userId == id);
            var mails = db.Mails.Where(m => m.from == user.login).ToList();
            return View(mails);
        }

        public ActionResult ParentIndex()
        {
            List<Mail> mails = new List<Mail>();
            List<StudentClass> classes = new List<StudentClass>();

            var username = HttpContext.Session.GetString("UserId");
            if (int.TryParse(username, out int id))
            {
                var kids = db.parentWithKids
                     .Where(p => p.parentId == id)
                     .Select(p => p.studentId)
                     .ToList();

                var classIds = db.StudentWithClasses
                                 .Where(sc => kids.Contains(sc.studentId))
                                 .Select(sc => sc.studentClassId)
                                 .Distinct()
                                 .ToList();

                mails = db.Mails
                          .Where(m => classIds.Contains(m.toClassId))
                          .ToList();


            }

            return View(mails);
        }

        // GET: Mail/Details/5
        public ActionResult Details(int id)
        {
            var mail = db.Mails.Find(id);
            if (mail == null) return RedirectToAction("Index");
            return View(mail);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Mail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("subject, body, toClassId")] Mail mail)
        {
            ModelState.Remove("from");
            if (ModelState.IsValid)
            {
                var username = HttpContext.Session.GetString("UserId");
                int.TryParse(username, out int id);
                var user = db.User.FirstOrDefault(u => u.userId == id);
                mail.from = user.login;
                mail.send = DateTime.Now;
                db.Mails.Add(mail);
                db.SaveChanges();

                // Pobierz rodziców przypisanych do klasy
                var parentEmails = db.parentWithKids
                                     .Join(db.StudentWithClasses,
                                           pwk => pwk.studentId,
                                           swc => swc.studentId,
                                           (pwk, swc) => new { pwk.parentId, swc.studentClassId })
                                     .Where(x => x.studentClassId == mail.toClassId)
                                     .Select(x => db.User.FirstOrDefault(u => u.userId == x.parentId).email)
                                     .Distinct()
                                     .ToList();

                // Wyślij e-mail do każdego rodzica
                foreach (var email in parentEmails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        await SendEmailAsync(email, "Nowa wiadomość w dzienniku", "Masz nową wiadomość w dzienniku.");
                    }
                }

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

        // GET: Mail/Edit/5
        public ActionResult Edit(int id)
        {
            var mail = db.Mails.Find(id);
            if (mail == null)
                return RedirectToAction("Index");
            else
                return View(mail);
        }

        // POST: Mail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("mailId, subject, from, body, toClassId")] Mail mail)
        {
            ModelState.Remove("toClassId");
            ModelState.Remove("mailId");
            ModelState.Remove("from");
            if (ModelState.IsValid)
            {
                mail.send = DateTime.Now;
                db.Entry(mail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mail);
        }

        // GET: Mail/Delete/5
        public ActionResult Delete(int id)
        {
            var mail = db.Mails.Find(id);
            if (mail == null) return RedirectToAction("Index");
            return View(mail);
        }

        // POST: Mail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var mail = db.Mails.Find(id);
            if (mail != null)
            {
                db.Mails.Remove(mail);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
