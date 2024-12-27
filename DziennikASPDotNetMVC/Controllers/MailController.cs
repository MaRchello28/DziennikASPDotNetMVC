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
            var mails = db.Mails.ToList();
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
        public ActionResult Create([Bind("subject, body, from, to, send")] Mail mail)
        {
            if (ModelState.IsValid)
            {
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
        public ActionResult Edit([Bind("mailId, subject, body, from, to, read, send")] Mail mail)
        {
            if (ModelState.IsValid)
            {
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
