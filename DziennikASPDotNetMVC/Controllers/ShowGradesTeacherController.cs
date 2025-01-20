using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net.Mail;
using System.Net;

public class ShowGradesTeacherController : Controller
{
    private readonly MyDbContext _context;

    public ShowGradesTeacherController(MyDbContext context)
    {
        _context = context;
    }

    // Akcja do wyświetlania tabeli ocen
    [HttpGet]
    public IActionResult SelectedParameters()
    {
        var teacherId = 8; // Przykładowe ID nauczyciela, może być dynamiczne

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
            Subjects = subjects,
            SelectedStudents = new List<int>(),
            SelectedSubjects = new List<int>()   
        };

        return View(model);
    }


    // Akcja do generowania tabeli ocen na podstawie wybranych uczniów i przedmiotów
    [HttpPost]
    public IActionResult ShowGrades(ShowGradesViewModel model, DateTime startDate, DateTime endDate)
    {
        if (model.SelectedStudents == null || !model.SelectedStudents.Any() ||
            model.SelectedSubjects == null || !model.SelectedSubjects.Any())
        {
            model.ErrorMessage = "Musisz wybrać uczniów i przedmioty.";
            return View(model);
        }

        var selectedStudentsData = _context.User
            .Where(u => model.SelectedStudents.Contains(u.userId))
            .ToList();

        var selectedSubjectsData = _context.Subjects
            .Where(s => model.SelectedSubjects.Contains(s.subjectId))
            .ToList();

        var gradesTable = selectedStudentsData.Select(student =>
        {
            dynamic studentRow = new ExpandoObject();
            studentRow.Student = $"{student.name} {student.surname}";

            studentRow.GradesBySubject = selectedSubjectsData.Select(subject =>
            {
                dynamic subjectRow = new ExpandoObject();
                subjectRow.SubjectName = subject.name;
                subjectRow.Grades = _context.Grades
                    .Where(g => g.subjectId == subject.subjectId && g.studentId == student.userId && g.time >= startDate && g.time <= endDate)
                    .Select(g => g.value.ToString())
                    .ToList();

                return subjectRow;
            }).ToList();

            return studentRow;
        }).ToList();

        model.GradesTable = gradesTable;
        model.Subjects = selectedSubjectsData;

        return View(model);
    }


    // Akcja do wysyłania e-maila z wynikami
    [HttpPost]
    public async Task<IActionResult> SendGradesEmail(ShowGradesViewModel model)
    {
        if (model.SelectedStudents == null || !model.SelectedStudents.Any() ||
            model.SelectedSubjects == null || !model.SelectedSubjects.Any())
        {
            model.ErrorMessage = "Musisz wybrać uczniów i przedmioty.";
            return View("ShowGrades", model);
        }

        var selectedStudentsData = _context.User
            .Where(u => model.SelectedStudents.Contains(u.userId))
            .ToList();

        var selectedSubjectsData = _context.Subjects
            .Where(s => model.SelectedSubjects.Contains(s.subjectId))
            .ToList();

        var gradesTable = selectedStudentsData.Select(student =>
        {
            dynamic studentRow = new ExpandoObject();
            studentRow.Student = $"{student.name} {student.surname}";

            studentRow.GradesBySubject = selectedSubjectsData.Select(subject =>
            {
                dynamic subjectRow = new ExpandoObject();
                subjectRow.SubjectName = subject.name;
                subjectRow.Grades = _context.Grades
                    .Where(g => g.subjectId == subject.subjectId && g.studentId == student.userId)
                    .Select(g => g.value.ToString())
                    .ToList();

                return subjectRow;
            }).ToList();

            return studentRow;
        }).ToList();

        // Generowanie wiadomości e-mail
        var emailMessage = GenerateGradesEmail(selectedStudentsData, selectedSubjectsData, gradesTable);

        var parentEmail = GetParentEmail(selectedStudentsData);
        if (!string.IsNullOrEmpty(parentEmail))
        {
            await SendEmailAsync(parentEmail, "Oceny ucznia", emailMessage);
        }

        model.GradesTable = gradesTable;
        model.Subjects = selectedSubjectsData;

        return RedirectToAction("TeacherView", "HomeRoles");
    }


    private string GenerateGradesEmail(List<User> students, List<Subject> subjects, List<dynamic> gradesTable)
    {
        var emailBody = "<h3>Wyniki uczniów</h3><table border='1' style='border-collapse: collapse;'><tr><th>Uczeń</th>";

        foreach (var subject in subjects)
        {
            emailBody += $"<th>{subject.name}</th>";
        }
        emailBody += "</tr>";

        foreach (var studentRow in gradesTable)
        {
            emailBody += $"<tr><td>{studentRow.Student}</td>";
            foreach (var subject in studentRow.GradesBySubject)
            {
                var grades = subject.Grades != null && subject.Grades.Count > 0
                    ? string.Join(", ", subject.Grades)
                    : "Brak ocen";
                emailBody += $"<td>{grades}</td>";
            }
            emailBody += "</tr>";
        }
        emailBody += "</table>";

        return emailBody;
    }

    private string GetParentEmail(List<User> students)
    {
        var student = students.First();
        var parentWithKid = _context.parentWithKids.FirstOrDefault(p => p.studentId == student.userId);

        if (parentWithKid != null)
        {
            var parent = _context.User.FirstOrDefault(u => u.userId == parentWithKid.parentId);
            return parent?.email;
        }

        return null;
    }

    private async Task SendEmailAsync(string email, string subject, string message)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("mateoprokop@gmail.com", "zhnk exxu vrwt njfq"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("mateoprokop@gmail.com"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
