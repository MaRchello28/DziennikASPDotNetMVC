using DziennikASPDotNetMVC.Models;
using DziennikASPDotNetMVC.Models.LinkTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace DziennikASPDotNetMVC.Controllers
{
    public class QuizController : Controller
    {
        MyDbContext db;
        public QuizController(MyDbContext db) { this.db = db; }


        //Dla uczniów
        public IActionResult StudentIndex()
        {
            var username = HttpContext.Session.GetString("UserId");
            int.TryParse(username, out int id);

            var StudentWithClasses = db.StudentWithClasses.FirstOrDefault(s => s.studentId == id);

            if(StudentWithClasses != null)
            {
                var studentClass = db.StudentClasses.FirstOrDefault(s => s.studentClassId == StudentWithClasses.studentClassId);

                if(studentClass != null)
                {
                    var currentDateTime = DateTime.Now;

                    var quizzesForStudentClass = db.QuizAndSelectedClasseses
                        .Where(q => q.studentClassId == studentClass.studentClassId &&
                                    q.availableFrom <= currentDateTime &&
                                    q.availableTo >= currentDateTime)
                        .Include(q => q.quiz)
                        .ToList();
                    return View(quizzesForStudentClass);
                }

            }


            return View();
        }

        [HttpGet]
        public IActionResult StartQuiz(int quizId)
        {
            var quiz = db.Quizzes.FirstOrDefault(q => q.quizId == quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            var questions = db.Questions.Where(q => q.quizId == quizId).ToList();

            Random rng = new Random();
            questions = questions.OrderBy(q => rng.Next()).ToList(); // Tasowanie pytań

            var questionsJson = JsonConvert.SerializeObject(questions);
            HttpContext.Session.Set("shuffledQuestions", Encoding.UTF8.GetBytes(questionsJson)); // Zapisz przetasowane pytania w sesji

            if (HttpContext.Session.GetInt32("currentQuestionIndex") == null)
            {
                HttpContext.Session.SetInt32("currentQuestionIndex", 0); // Zainicjalizuj indeks na 0, jeśli jeszcze nie istnieje
            }

            var currentQuestionIndex = HttpContext.Session.GetInt32("currentQuestionIndex") ?? 0;
            var currentQuestion = questions.ElementAtOrDefault(currentQuestionIndex);

            if (currentQuestion == null)
            {
                return NotFound();
            }

            var model = new StartQuizViewModel
            {
                QuizId = quizId,
                CurrentQuestionId = currentQuestion.questionId,
                CurrentQuestionText = currentQuestion.question,
                CurrentAnswerA = currentQuestion.answerA,
                CurrentAnswerB = currentQuestion.answerB,
                CurrentAnswerC = currentQuestion.answerC,
                CurrentAnswerD = currentQuestion.answerD,
                TimeRemaining = TimeSpan.FromMinutes(10) // Czas na odpowiedź
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnswerQuestion(int quizId, string selectedAnswer)
        {
            // Pobierz zapisane pytania z sesji
            var shuffledQuestionsJson = HttpContext.Session.GetString("shuffledQuestions");
            if (string.IsNullOrEmpty(shuffledQuestionsJson))
            {
                return NotFound();
            }

            // Zdeserializuj pytania
            var shuffledQuestions = JsonConvert.DeserializeObject<List<Question>>(shuffledQuestionsJson);
            var currentQuestionIndex = HttpContext.Session.GetInt32("currentQuestionIndex") ?? 0;

            // Pobierz aktualne pytanie
            var currentQuestion = shuffledQuestions.ElementAtOrDefault(currentQuestionIndex);

            if (currentQuestion == null)
            {
                return RedirectToAction("QuizResults", new { quizId = quizId });
            }

            // Sprawdź, czy odpowiedź użytkownika jest poprawna
            bool isCorrect = selectedAnswer == currentQuestion.correctAnswer;

            // Zwiększ punkty w sesji, jeśli odpowiedź jest poprawna
            var totalPoints = HttpContext.Session.GetInt32("totalPoints") ?? 0;
            if (isCorrect)
            {
                totalPoints += currentQuestion.points; // Dodaj punktację pytania
            }
            HttpContext.Session.SetInt32("totalPoints", totalPoints);

            // Zwiększ indeks pytania w sesji
            currentQuestionIndex++;
            HttpContext.Session.SetInt32("currentQuestionIndex", currentQuestionIndex);

            // Jeśli pytanie to nie ostatnie, wyświetl kolejne pytanie
            if (currentQuestionIndex < shuffledQuestions.Count)
            {
                var nextQuestion = shuffledQuestions[currentQuestionIndex];
                var model = new StartQuizViewModel
                {
                    QuizId = quizId,
                    CurrentQuestionId = nextQuestion.questionId,
                    CurrentQuestionText = nextQuestion.question,
                    CurrentAnswerA = nextQuestion.answerA,
                    CurrentAnswerB = nextQuestion.answerB,
                    CurrentAnswerC = nextQuestion.answerC,
                    CurrentAnswerD = nextQuestion.answerD,
                    TimeRemaining = TimeSpan.FromMinutes(10) // Możesz dostosować czas
                };

                return View("StartQuiz", model);
            }
            else
            {
                // Jeśli wszystkie pytania zostały odpowiedziane, przejdź do wyników
                return RedirectToAction("QuizResults", new { quizId = quizId });
            }
        }

        public IActionResult QuizResults(int quizId)
        {
            var totalPoints = HttpContext.Session.GetInt32("totalPoints") ?? 0;
            HttpContext.Session.Remove("currentQuestionId");
            HttpContext.Session.Remove("totalPoints");

            var quiz = db.Quizzes.FirstOrDefault(q => q.quizId == quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            if(quiz.generateGrade)
            {
                double percent = 100 * totalPoints / quiz.maxPoints;
                double gradeValue;
                if(percent <30) { gradeValue = 1; }
                else if(percent < 50) {  gradeValue = 2; }
                else if (percent < 60) { gradeValue = 3; }
                else if (percent < 70) { gradeValue = 4; }
                else if (percent < 80) { gradeValue = 5; }
                else { gradeValue = 6; }
                var username = HttpContext.Session.GetString("UserId");
                int.TryParse(username, out int id);
                Grade grade = new Grade(gradeValue, 2, "Quiz: "+quiz.name, id, quiz.teacherId, quiz.subjectId);
                db.Grades.Add(grade);
                db.SaveChanges();
            }

            var model = new QuizResultsViewModel
            {
                QuizId = quizId,
                TotalPoints = totalPoints
            };

            return View(model);
        }




        //Dla nauczycieli

        [Authorize(Policy = "TeacherOnly")]
        public IActionResult TeacherIndex()
        {
            var username = HttpContext.Session.GetString("UserId");
            int.TryParse(username, out int id);

            var quizzes = db.Quizzes.Where(q => q.teacherId == id).ToList();

            foreach (var quiz in quizzes)
            {
                QuizMaxPoints(quiz.quizId);
            }

            db.SaveChanges();
            return View(quizzes);
        }
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult Create()
        {
            var subjects = db.Subjects.ToList();
            ViewBag.Subjects = new SelectList(subjects, "subjectId", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult Create(Quiz quiz)
        {
            var username = HttpContext.Session.GetString("UserId");
            int.TryParse(username, out int id);
            quiz.teacherId = id;

            if (ModelState.IsValid)
            {
                db.Quizzes.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("TeacherIndex");
            }

            ViewBag.Subjects = new SelectList(db.Subjects.ToList(), "Id", "Name", quiz.subjectId);
            return View(quiz);
        }

        [Authorize(Policy = "TeacherOnly")]
        public IActionResult ManageQuestions(int quizId)
        {
            var quiz = db.Quizzes.FirstOrDefault(q => q.quizId == quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            var questions = db.Questions.Where(q => q.quizId == quizId).ToList();
            ViewBag.Quiz = quiz;
            return View(questions);
        }

        [Authorize(Policy = "TeacherOnly")]
        public IActionResult CreateQuestion(int quizId)
        {
            var quiz = db.Quizzes.FirstOrDefault(q => q.quizId == quizId);
            if (quiz == null)
            {
                return NotFound();
            }
            var question = new Question { quizId = quizId };
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult CreateQuestion(Question quest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(quest);
            }

            db.Questions.Add(quest);
            QuizMaxPoints(quest.quizId);
            db.SaveChanges();
            return RedirectToAction("ManageQuestions", new { quizId = quest.quizId });
        }

        [Authorize(Policy = "TeacherOnly")]
        public IActionResult EditQuestion(int id)
        {
            var question = db.Questions.FirstOrDefault(q => q.questionId == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult EditQuestion(Question quest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(quest).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ManageQuestions", new { quizId = quest.quizId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Nie można zapisać zmian. Element został zmodyfikowany lub usunięty przez innego użytkownika.");
                }
            }
            return View(quest);
        }
        [Authorize(Policy = "TeacherOnly")]

        public IActionResult DeleteQuestion(int id)
        {
            var question = db.Questions.FirstOrDefault(q => q.questionId == id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["QuizId"] = question.quizId;
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult DeleteQuestionConfirmed(Question quest)
        {
            if (quest == null)
            {
                return NotFound();
            }

            try
            {
                var questionToDelete = db.Questions.FirstOrDefault(q => q.questionId == quest.questionId);
                if (questionToDelete == null)
                {
                    return NotFound();
                }

                db.Questions.Remove(questionToDelete);
                db.SaveChanges();
                return RedirectToAction("ManageQuestions", new { quizId = questionToDelete.quizId });
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Nie można usunąć pytania. Spróbuj ponownie.");
                return View(quest);
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        public void QuizMaxPoints(int quizId)
        {
            var questions = db.Questions.Where(q => q.quizId == quizId).ToList();
            int maxPoints = 0;
            var quiz = db.Quizzes.FirstOrDefault(q => q.quizId == quizId);
            foreach(Question quest in questions)
            {
                maxPoints += quest.points;
            }
            if (quiz != null)
            {
                quiz.maxPoints = maxPoints;
                db.Entry(quiz).Property(q => q.maxPoints).IsModified = true;
                //db.SaveChanges();
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        public IActionResult SelectClass(int quizId)
        {
            var studentClasses = db.StudentClasses.ToList();
            ViewBag.QuizId = quizId;
            return View(studentClasses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult AssignClassToQuiz(int quizId, int studentClassId, DateTime? availableFrom, DateTime? availableTo)
        {
            if (ModelState.IsValid)
            {
                var quizAndClass = new QuizAndSelectedClass
                {
                    quizId = quizId,
                    studentClassId = studentClassId,
                    availableFrom = availableFrom,
                    availableTo = availableTo
                };

                db.QuizAndSelectedClasseses.Add(quizAndClass);
                db.SaveChanges();

                return RedirectToAction("TeacherIndex");
            }

            var studentClasses = db.StudentClasses.ToList();
            ViewBag.QuizId = quizId;
            return View("SelectClass", studentClasses);
        }

        [Authorize(Policy = "TeacherOnly")]

        public IActionResult ManageQuizClasses(int quizId)
        {
            var quizAndClasses = db.QuizAndSelectedClasseses
                .Where(qs => qs.quizId == quizId)
                .ToList();

            return View(quizAndClasses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TeacherOnly")]
        public IActionResult DeleteQuizClass(int id)
        {
            var quizAndClass = db.QuizAndSelectedClasseses.FirstOrDefault(qs => qs.Id == id);
            if (quizAndClass == null)
            {
                return NotFound();
            }

            db.QuizAndSelectedClasseses.Remove(quizAndClass);
            db.SaveChanges();

            return RedirectToAction("ManageQuizClasses", new { quizId = quizAndClass.quizId });
        }
    }
}
