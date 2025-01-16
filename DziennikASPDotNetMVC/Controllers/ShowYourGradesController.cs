using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DziennikASPDotNetMVC.Controllers
{
    class ShowStudentGradesModel
    {
        public string name { get; set; }
        public List<Grade> grades { get; set; }

        public ShowStudentGradesModel(string name, List<Grade> grades)
        {
            this.name = name;
            this.grades = grades;
        }
    }

    public class ShowYourGradesController : Controller
    {
        private readonly MyDbContext db;

        public ShowYourGradesController(MyDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Subject> subjects = new List<Subject>();
            List<ShowStudentGradesModel> gradesList = new List<ShowStudentGradesModel>();
            string studentId = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(studentId))
            {
                int id = int.Parse(studentId);
                int loggedInStudentClassId = GetLoggedInStudentClassId(id);
                subjects = db.Subjects
                     .Where(subject => db.Sessions
                                         .Any(session => session.subjectId == subject.subjectId && session.studentClassId == loggedInStudentClassId))
                     .ToList();
                foreach(Subject subject in subjects)
                {
                    List<Grade> grades = db.Grades.Where(g => g.subjectId == subject.subjectId && g.studentId == id).ToList();
                    gradesList.Add(new ShowStudentGradesModel(subject.name, grades));

                }
            }

            return View(gradesList);
        }

        private int GetLoggedInStudentClassId(int studentId)
        {
            var studentClass = db.StudentWithClasses.FirstOrDefault(s => s.studentId == studentId);
            return studentClass?.studentClassId ?? -1;
        }
    }
}