using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class SessionController : Controller
    {
        private readonly MyDbContext db; 
        public SessionController(MyDbContext db)
        {
            this.db = db;
        }
        // GET: Session
        public ActionResult Index()
        {
            var sessions = db.Sessions.ToList();
            return View(sessions);
        }

        // GET: Session/Details/5
        public ActionResult Details(int id)
        {
            var session = db.Sessions.Find(id);
            if (session == null) return RedirectToAction("Index");
            return View(session);
        }

        // GET: Session/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Session/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("sessionId, subjectId, teacherId, dayOfWeek, hourFrom, hourTo, replacement, sala")]Session session)
        {
            ModelState.Remove("lessons");

            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Session/Edit/5
        public ActionResult Edit(int id)
        {
            var session = db.Sessions.Find(id);
            if (session == null)
                return RedirectToAction("Index");
            else
                return View(session);
        }

        // POST: Session/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session)
        {
            ModelState.Remove("lessons");
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Session/Delete/5
        public ActionResult Delete(int id)
        {
            var session = db.Sessions.Find(id);
            if (session == null) return RedirectToAction("Index");
            return View(session);
        }

        // POST: Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}