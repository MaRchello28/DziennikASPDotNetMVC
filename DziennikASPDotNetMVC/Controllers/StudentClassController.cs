using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DziennikASPDotNetMVC.Controllers
{
    public class StudentClassController : Controller
    {
        private readonly MyDbContext db;

        public StudentClassController(MyDbContext db)
        {
            this.db = db;
        }

        // GET: Plan lekcji dla ucznia
        public IActionResult Index()
        {
            int userId;
            var userIdString = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdString, out userId))
            {
                return BadRequest("Nieprawidłowy identyfikator użytkownika.");
            }

            var studentWithClass = db.StudentWithClasses
                .Include(swc => swc.StudentClass)
                .Include(swc => swc.Student)
                .FirstOrDefault(swc => swc.studentId == userId);

            if (studentWithClass == null)
            {
                return NotFound("Nie znaleziono studenta.");
            }

            var studentClass = studentWithClass.StudentClass;
            if (studentClass == null)
            {
                return NotFound("Klasa ucznia nie została znaleziona.");
            }

            var sessions = db.Sessions
                .Where(s => s.studentClassId == studentClass.studentClassId)
                .ToList();

            if (sessions == null || !sessions.Any())
            {
                return NotFound("Nie znaleziono sesji dla tej klasy.");
            }

            // Zmieniamy anonimowy typ na instancję SessionDetails
            var sessionDetails = sessions.Select(session => new SessionDetails
            {
                SubjectName = db.Subjects
                    .Where(subject => subject.subjectId == session.subjectId)
                    .Select(subject => subject.name)
                    .FirstOrDefault() ?? "Nieznany przedmiot",

                TeacherName = db.User
                    .Where(user => user.userId == session.teacherId && user.type == "teacher")
                    .Select(user => user.name)
                    .FirstOrDefault() ?? "Nieznany nauczyciel",

                TeacherSurname = db.User
                    .Where(user => user.userId == session.teacherId && user.type == "teacher")
                    .Select(user => user.surname)
                    .FirstOrDefault() ?? "Nieznany nauczyciel",

                Room = session.sala.ToString(),
                HourFrom = session.hourFrom,
                HourTo = session.hourTo,
                DayOfWeek = session.dayOfTheWeek
            }).ToList();

            // Grupowanie sesji według dnia tygodnia
            var groupedSessions = sessionDetails
                .GroupBy(s => s.DayOfWeek)
                .OrderBy(g => g.Key)
                .ToList();

            var model = new ShowClassScheduleViewModel
            {
                StudentName = studentWithClass.Student.name + " " + studentWithClass.Student.surname,
                Sessions = groupedSessions
                    .Select(g => new DaySchedule
                    {
                        DayOfWeek = g.Key,
                        Sessions = g.OrderBy(s => s.HourFrom).ToList()
                    }).ToList()
            };

            return View(model);
        }


    }

}
