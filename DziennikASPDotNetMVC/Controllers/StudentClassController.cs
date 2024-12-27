using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    public class StudentClassController : Controller
    {
        private readonly MyDbContext db;
        public StudentClassController(MyDbContext db)
        {
            this.db = db;
        }
        // GET: StudentClass
        public ActionResult Index()
        {
            var studentClass = db.StudentClasses.ToList();
            return View(studentClass);
        }
        // GET: studentClass/Details/5
        public ActionResult Details(int id)
        {
            var studentClass = db.StudentClasses.Find(id);
            if (studentClass == null) return RedirectToAction("Index");
            return View(studentClass);
        }

        // GET: studentClass/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: studentClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("number, letter, teacherId")] StudentClass studentClass)
        {
            ModelState.Remove("classes");

            bool studentClassExist = db.StudentClasses.Any(s => s.studentClassId == studentClass.studentClassId);
            bool teacherExists = db.User.Any(t => t.userId == studentClass.teacherId && Equals(t.type, "teacher"));
            bool correctNumberAndLetter = db.StudentClasses.Any(c => c.number == studentClass.number && c.letter == studentClass.letter);

            if (ModelState.IsValid)
            {
                db.StudentClasses.Add(studentClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: studentClass/Edit/5
        public ActionResult Edit(int id)
        {
            var studentClass = db.StudentClasses.Find(id);
            if (studentClass == null)
                return RedirectToAction("Index");
            else
                return View(studentClass);
        }

        // POST: studentClass/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session studentClass)
        {
            ModelState.Remove("lessons");
            if (ModelState.IsValid)
            {
                db.Entry(studentClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentClass);
        }

        // GET: studentClass/Delete/5
        public ActionResult Delete(int id)
        {
            var studentClass = db.StudentClasses.Find(id);
            if (studentClass == null) return RedirectToAction("Index");
            return View(studentClass);
        }

        // POST: studentClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var studentClass = db.StudentClasses.Find(id);
            db.StudentClasses.Remove(studentClass);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
