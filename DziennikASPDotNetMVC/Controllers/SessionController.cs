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
            ViewBag.Subjects = db.Subjects.Select(s=> new { s.subjectId, s.name }).ToList();
            ViewBag.Teachers = db.User
                .Where(u => u.type == "teacher")
                .Select(u => new {
                    u.userId,
                    u.name,
                    u.surname,
                    Subjects = db.TeacherWithSubjects
                        .Where(tws => tws.teacherId == u.userId)
                        .Join(db.Subjects, tws => tws.subjectId, s => s.subjectId, (tws, s) => s.name)
                        .ToList()
                })
                .OrderBy(u => u.name)
                .ThenBy(u => u.surname)
                .ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("sessionId, subjectId, teacherId, dayOfTheWeek, hourFrom, hourTo, sala, studentClassId")] Session session)
        {
            ModelState.Remove("lessons");

            bool subjectExists = db.Subjects.Any(s => s.subjectId == session.subjectId);
            if (!subjectExists)
            {
                ModelState.AddModelError("subjectId", "Wybrany przedmiot nie istnieje");
            }

            bool teacherExists = db.User.Any(t => t.userId == session.teacherId && Equals(t.type, "teacher"));
            if (!teacherExists)
            {
                ModelState.AddModelError("teacherId", "Musisz wybrać nauczyciela prowadzącego");
            }

            bool correctDayOfWeek = session.dayOfTheWeek == DayOfWeek.Saturday || session.dayOfTheWeek == DayOfWeek.Sunday;
            if (correctDayOfWeek)
            {
                ModelState.AddModelError("dayOfTheWeek", "W weekend zajęcia nie mogą się odbywać");
            }

            bool correctHourFrom = db.HoursForLessons.Any(h => h.hourFrom == session.hourFrom);
            if (!correctHourFrom)
            {
                ModelState.AddModelError("hourFrom", "Nie istnieje taka godzina rozpoczęcia zajęć");
            }

            bool correctHourTo = db.HoursForLessons.Any(h => h.hourTo == session.hourTo);
            if (!correctHourTo)
            {
                ModelState.AddModelError("hourTo", "Nie istnieje taka godzina końca zajęć");
            }

            if (session.sala <= 0)
            {
                ModelState.AddModelError("sala", "Musisz wpisać sale");
            }

            if (session.studentClassId <= 0)
            {
                ModelState.AddModelError("StudentClass", "Musisz wpisać sale");
            }

            if (!db.StudentClasses.Any(s => s.studentClassId == session.studentClassId))
            {
                ModelState.AddModelError("StudentClass", "Nie istnieje id takiej klasy");
            }

            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Subjects = db.Subjects.Select(s => new { s.subjectId, s.name }).ToList();
            ViewBag.Teachers = db.User
                .Where(u => u.type == "teacher")
                .Select(u => new {
                    u.userId,
                    u.name,
                    u.surname,
                    Subjects = db.TeacherWithSubjects
                        .Where(tws => tws.teacherId == u.userId)
                        .Join(db.Subjects, tws => tws.subjectId, s => s.subjectId, (tws, s) => s.name)
                        .ToList()
                })
                .ToList();

            return View();
        }

        // GET: Session/Edit/5
        public ActionResult Edit(int id)
        {

            ViewBag.Subjects = db.Subjects.Select(s => new { s.subjectId, s.name }).ToList();
            ViewBag.Teachers = db.User
                .Where(u => u.type == "teacher")
                .Select(u => new {
                    u.userId,
                    u.name,
                    u.surname,
                    Subjects = db.TeacherWithSubjects
                        .Where(tws => tws.teacherId == u.userId)
                        .Join(db.Subjects, tws => tws.subjectId, s => s.subjectId, (tws, s) => s.name)
                        .ToList()
                })
                .OrderBy(u => u.name)
                .ThenBy(u => u.surname)
                .ToList();
            var session = db.Sessions.Find(id);
            if (session == null)
                return RedirectToAction("Index");
            else
                return View(session);
        }

        // POST: Session/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("sessionId, subjectId, teacherId, dayOfTheWeek, hourFrom, hourTo, sala")] Session session)
        {
            ModelState.Remove("lessons");
            bool subjectExists = db.Subjects.Any(s => s.subjectId == session.subjectId);
            if (!subjectExists)
            {
                ModelState.AddModelError("subjectId", "Wybrany przedmiot nie istnieje");
            }

            bool teacherExists = db.User.Any(t => t.userId == session.teacherId && Equals(t.type, "teacher"));
            if (!teacherExists)
            {
                ModelState.AddModelError("teacherId", "Musisz wybrać nauczyciela prowadzącego");
            }

            bool correctDayOfWeek = session.dayOfTheWeek == DayOfWeek.Saturday || session.dayOfTheWeek == DayOfWeek.Sunday;
            if (correctDayOfWeek)
            {
                ModelState.AddModelError("dayOfTheWeek", "W weekend zajęcia nie mogą się odbywać");
            }

            bool correctHourFrom = db.HoursForLessons.Any(h => h.hourFrom == session.hourFrom);
            if (!correctHourFrom)
            {
                ModelState.AddModelError("hourFrom", "Nie istnieje taka godzina rozpoczęcia zajęć");
            }

            bool correctHourTo = db.HoursForLessons.Any(h => h.hourTo == session.hourTo);
            if (!correctHourTo)
            {
                ModelState.AddModelError("hourTo", "Nie istnieje taka godzina końca zajęć");
            }

            if (session.sala == 0)
            {
                ModelState.AddModelError("hourTo", "Musisz wpisać sale");
            }

            if (ModelState.IsValid)
            {
                db.Sessions.Update(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Subjects = db.Subjects.Select(s => new { s.subjectId, s.name }).ToList();
            ViewBag.Teachers = db.User
                .Where(u => u.type == "teacher")
                .Select(u => new {
                    u.userId,
                    u.name,
                    u.surname,
                    Subjects = db.TeacherWithSubjects
                        .Where(tws => tws.teacherId == u.userId)
                        .Join(db.Subjects, tws => tws.subjectId, s => s.subjectId, (tws, s) => s.name)
                        .ToList()
                })
                .ToList();
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