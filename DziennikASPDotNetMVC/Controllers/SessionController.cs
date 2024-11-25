using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    public class SessionController : Controller
    {
        private readonly MyDbContext _context; 
        public SessionController(MyDbContext context)
        {
            this._context = context;
        }
        // GET: Session
        public ActionResult Index()
        {
            var sessions = _context.Sessions.Include("Lessons").ToList();
            return View(sessions);
        }

        // GET: Session/Details/5
        public ActionResult Details(int id)
        {
            var session = _context.Sessions.Find(id);
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
        public ActionResult Create(Session session)
        {
            session.hourFrom = TimeSpan.Parse(session.hourFrom.ToString());
            session.hourTo = TimeSpan.Parse(session.hourTo.ToString());

            _context.Sessions.Add(session);
            _context.SaveChanges();
            return View(session);
        }

        // GET: Session/Edit/5
        public ActionResult Edit(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session == null) return RedirectToAction("Index");
            return View(session);
        }

        // POST: Session/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(session).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Session/Delete/5
        public ActionResult Delete(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session == null) return RedirectToAction("Index");
            return View(session);
        }

        // POST: Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var session = _context.Sessions.Find(id);
            _context.Sessions.Remove(session);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}