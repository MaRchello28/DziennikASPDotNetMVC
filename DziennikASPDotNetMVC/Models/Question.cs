using System.ComponentModel.DataAnnotations;

namespace DziennikASPDotNetMVC.Models
{
    public class Question
    {
        public int questionId { get; set; }

        [Required(ErrorMessage = "Pole quizId jest wymagane.")]
        public int quizId { get; set; }

        [Required(ErrorMessage = "Pole pytanie jest wymagane.")]
        public string question { get; set; }

        [Required(ErrorMessage = "Pole odpowiedź A jest wymagane.")]
        public string answerA { get; set; }

        [Required(ErrorMessage = "Pole odpowiedź B jest wymagane.")]
        public string answerB { get; set; }

        [Required(ErrorMessage = "Pole odpowiedź C jest wymagane.")]
        public string answerC { get; set; }

        [Required(ErrorMessage = "Pole odpowiedź D jest wymagane.")]
        public string answerD { get; set; }

        [Required(ErrorMessage = "Pole prawidłowa odpowiedź jest wymagane.")]
        public string correctAnswer { get; set; }

        [Required(ErrorMessage = "Pole punkty jest wymagane.")]
        public int points { get; set; }

        public Question() { }
        public Question(int quizId, string question, string answerA, string answerB, string answerC, string answerD, 
            string correctAnswer, int points) 
        { 
            this.quizId = quizId;
            this.question = question;
            this.answerA = answerA;
            this.answerB = answerB;
            this.answerC = answerC;
            this.answerD = answerD;
            this.correctAnswer = correctAnswer;
            this.points = points;
        }
    }
}
