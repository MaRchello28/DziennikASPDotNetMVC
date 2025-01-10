using DziennikASPDotNetMVC.Models;
using DziennikASPDotNetMVC.Models.ForControllersOnly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DziennikASPDotNetMVC.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class SelectStudentToClass : Controller
    {
        private readonly MyDbContext db;
        private int classId;

        public SelectStudentToClass(MyDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var classes = db.StudentClasses.ToList();
            return View(classes);
        }

        public IActionResult SelectStudents(int id)
        {
            classId = id;
            ViewBag.classId = id;
            var students = db.User.Where(u => u.type == "student").ToList();

            var studentViewModels = students.Select(s => new StudentViewModel
            {
                UserId = s.userId,
                Name = s.name,
                Surname = s.surname,
                Login = s.login,
                IsAssignedToClass = db.StudentWithClasses.Any(z => z.studentClassId == id && z.studentId == s.userId)
            }).ToList();

            return View("AddStudentToClass", studentViewModels);
        }

        [HttpPost]
        public IActionResult SelectStudents(List<int> selectedStudents, int classId)
        {
            foreach (var studentId in selectedStudents)
            {
                bool isAlreadyAssigned = db.StudentWithClasses.Any(sc => sc.studentClassId == classId && sc.studentId == studentId);

                if (!isAlreadyAssigned)
                {
                    db.StudentWithClasses.Add(new StudentWithClass
                    {
                        studentClassId = classId,
                        studentId = studentId
                    });
                }
            }

            var studentsInClass = db.StudentWithClasses.Where(sc => sc.studentClassId == classId).ToList();
            foreach (var studentInClass in studentsInClass)
            {
                if (!selectedStudents.Contains(studentInClass.studentId))
                {
                    db.StudentWithClasses.Remove(studentInClass);
                }
            }

            db.SaveChanges();

            return RedirectToAction("AdminView", "HomeRoles");
        }
    }
}
