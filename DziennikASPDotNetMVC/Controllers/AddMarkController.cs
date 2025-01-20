using DziennikASPDotNetMVC.Models;
using DziennikASPDotNetMVC.Models.LinkTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DziennikASPDotNetMVC.Controllers
{
    public class AddMarkController : Controller
    {
        private readonly MyDbContext _db;

        public AddMarkController(MyDbContext db)
        {
            _db = db;
        }

        // Action to show the form for selecting class and subject
        public IActionResult Index()
        {
            var teacherId = 8; // This should be dynamically set (e.g., from logged-in teacher)

            // Get the subjects that the teacher is assigned to
            var teacherSubjects = _db.TeacherWithSubjects
                                      .Where(ts => ts.teacherId == teacherId)
                                      .ToList();

            // Fetch the names of the subjects from the Subject table
            var subjects = teacherSubjects
                           .Select(ts => new { ts.subjectId, SubjectName = _db.Subjects.FirstOrDefault(s => s.subjectId == ts.subjectId)?.name })
                           .ToList();

            ViewBag.Subjects = subjects;
            ViewBag.Classes = _db.StudentClasses.Where(sc => sc.teacherId == teacherId).ToList();

            return View();
        }

        // Action to show the students of the selected class and subject
        [HttpPost]
        public IActionResult StudentsInClassAndSubject(int studentClassId, int subjectId)
        {
            var students = _db.StudentWithClasses
                              .Where(sc => sc.studentClassId == studentClassId)
                              .Include(sc => sc.Student)
                              .ToList();

            ViewBag.Students = students.Select(sc => sc.Student).ToList();
            ViewBag.SubjectId = subjectId;
            ViewBag.StudentClassId = studentClassId;

            return View();
        }

        [HttpPost]
        public IActionResult SaveGrades(int studentClassId, int subjectId, Dictionary<int, string> grades)
        {
            var teacherId = 8; // This should be dynamically set (e.g., from logged-in teacher)

            // Loop through each student and their corresponding grade data
            foreach (var gradeEntry in grades)
            {
                var studentId = gradeEntry.Key;

                // Retrieve the values for the grade, wage, and description
                var value = Convert.ToDouble(Request.Form[$"grades[{studentId}].value"]);
                var wage = Convert.ToInt32(Request.Form[$"grades[{studentId}].wage"]);
                var description = Request.Form[$"grades[{studentId}].description"];

                // Create and populate the grade object
                var grade = new Grade
                {
                    studentId = studentId,
                    teacherId = teacherId,
                    subjectId = subjectId,
                    value = value,
                    wage = wage,
                    description = description,
                    time = DateTime.Now
                };

                // Add the grade to the context
                _db.Grades.Add(grade);
            }

            // Save changes to the database
            _db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
