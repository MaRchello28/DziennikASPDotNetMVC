using DziennikASPDotNetMVC.Models;
using DziennikASPDotNetMVC.Models.LinkTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DziennikASPDotNetMVC.Controllers
{
    public class StudentWithClassController : Controller
    {
        private readonly MyDbContext db;

        public StudentWithClassController(MyDbContext db)
        {
            this.db = db;
        }

        // GET: StudentClass/Index
        public ActionResult Index()
        {
            // Pobieramy wszystkie klasy
            var studentClasses = db.StudentClasses.ToList();

            // Zwracamy listę klas do widoku
            return View(studentClasses);
        }

        // GET: StudentClass/Details/5
        public ActionResult Details(int id)
        {
            // Pobieramy klasę o danym ID
            var studentClass = db.StudentClasses
            .Include(sc => sc.StudentWithClasses) // Ładujemy powiązania
            .ThenInclude(sw => sw.Student) // Ładujemy studentów przez StudentWithClass
            .FirstOrDefault(sc => sc.studentClassId == id);

            if (studentClass == null)
                return RedirectToAction("Index");

            // Zwracamy klasę i jej uczniów do widoku
            return View(studentClass);
        }

        // GET: StudentClass/Assign/5
        public ActionResult Assign(int id)
        {
            // Pobieramy klasę o danym ID
            var studentClass = db.StudentClasses.Find(id);
            if (studentClass == null)
                return RedirectToAction("Index", "StudentWithClass");
            // Pobieramy wszystkich studentów
            var students = db.User.Where(s => s.type == "student").ToList();

            // Zwracamy dane do widoku
            ViewBag.Students = new SelectList(students, "userId", "name"); // Zakładając, że masz pole fullName w User
            return View(studentClass);
        }

        // POST: StudentClass/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(int id, int studentId)
        {
            // Pobieramy klasę o danym ID
            var studentClass = db.StudentClasses.Find(id);
            if (studentClass == null)
                return RedirectToAction("Index", "StudentWithClass");

            // Pobieramy studenta
            var student = db.User.Find(studentId);
            if (student == null)
                return RedirectToAction("Index", "StudentWithClass");

            // Dodajemy studenta do klasy
            var studentWithClass = new DziennikASPDotNetMVC.Models.StudentWithClass
            {
                studentClassId = studentClass.studentClassId,
                studentId = student.userId
            };
            db.StudentWithClasses.Add(studentWithClass);
            db.SaveChanges();


            return RedirectToAction("Index", "StudentWithClass");
        }

        // GET: StudentWithClass/Remove/5
        public ActionResult Remove(int id)
        {
            var studentWithClass = db.StudentWithClasses.Find(id);
            if (studentWithClass == null)
                return RedirectToAction("Index", "StudentWithClass");

            return View(studentWithClass);
        }

        // POST: StudentWithClass/Remove/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int id)
        {
            var studentWithClass = db.StudentWithClasses.FirstOrDefault(s => s.studentId == id);
            db.StudentWithClasses.Remove(studentWithClass);
            db.SaveChanges();

            return RedirectToAction("Index", "StudentWithClass");
        }
    }
}
