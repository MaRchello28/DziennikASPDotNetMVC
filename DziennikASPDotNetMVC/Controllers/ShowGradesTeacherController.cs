using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DziennikASPDotNetMVC.Controllers
{
    public class ShowGradesTeacherController : Controller
    {
        private readonly MyDbContext _context;

        public ShowGradesTeacherController(MyDbContext context)
        {
            _context = context;
        }

        // Widok do wyboru uczniów i przedmiotów
        public IActionResult SelectedParameters()
        {
            var teacherId = 8; // Przykładowe ID nauczyciela, może być dynamiczne

            // Pobieranie przedmiotów i uczniów
            var subjects = _context.Subjects
                .Where(s => _context.Sessions
                    .Any(session => session.teacherId == teacherId && session.subjectId == s.subjectId))
                .ToList();

            var students = _context.User
                .Where(u => u.type == "student" &&
                    _context.StudentWithClasses
                        .Any(sw => _context.Sessions
                            .Any(session => session.teacherId == teacherId && session.studentClassId == sw.studentClassId) &&
                            sw.studentId == u.userId))
                .ToList();

            var model = new ShowGradesViewModel
            {
                Students = students,
                Subjects = subjects
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult ShowGrades(ShowGradesViewModel model, DateTime startDate, DateTime endDate)
        {
            if (model.SelectedStudents == null || !model.SelectedStudents.Any() || model.SelectedSubjects == null || !model.SelectedSubjects.Any())
            {
                model.ErrorMessage = "Musisz wybrać uczniów i przedmioty.";
                return View(model); // Zwracamy widok z błędem
            }

            // Pobieranie danych uczniów na podstawie wybranych studentów
            var selectedStudentsData = _context.User
                .Where(u => model.SelectedStudents.Contains(u.userId))
                .ToList();

            // Pobieranie danych przedmiotów na podstawie wybranych przedmiotów
            var selectedSubjectsData = _context.Subjects
                .Where(s => model.SelectedSubjects.Contains(s.subjectId))
                .ToList();

            // Generowanie tabeli ocen
            var gradesTable = selectedStudentsData.Select(student => new
            {
                Student = $"{student.name} {student.surname}",
                GradesBySubject = selectedSubjectsData.Select(subject => new
                {
                    SubjectName = subject.name,
                    Grades = _context.Grades
                        .Where(g => g.subjectId == subject.subjectId && g.studentId == student.userId && g.time >= startDate && g.time <= endDate)
                        .Select(g => g.value.ToString())  // Zamieniamy oceny na stringi od razu
                        .ToList() // Przechowujemy je jako List<string>
                }).ToList()
            }).ToList();

            // Sprawdzamy, czy tabela ocen zawiera dane
            if (!gradesTable.Any())
            {
                model.ErrorMessage = "Brak ocen w wybranym okresie."; // Ustawiamy komunikat błędu w modelu
                return View(model); // Zwracamy widok z komunikatem błędu
            }

            // Przekazywanie danych do widoku
            model.GradesTable = gradesTable.Cast<dynamic>().ToList();
            model.Subjects = selectedSubjectsData;

            return View(model); // Zwracamy model z danymi do widoku
        }


    }
}
