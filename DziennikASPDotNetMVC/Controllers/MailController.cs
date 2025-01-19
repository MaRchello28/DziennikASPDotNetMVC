using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: Mail/Details/5
        public ActionResult Details(int id)
        {
            var mail = db.Mails.Find(id);
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
        public ActionResult Create([Bind("subject, body, toClassId")] Mail mail)
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
                return RedirectToAction("Index");
            }

            return View(mail);
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
