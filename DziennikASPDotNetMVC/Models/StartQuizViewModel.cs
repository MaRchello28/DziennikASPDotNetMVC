namespace DziennikASPDotNetMVC.Models
{
    public class StartQuizViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public TimeSpan TimeRemaining { get; set; }

        // Dane dla aktualnego pytania
        public int CurrentQuestionId { get; set; }
        public string CurrentQuestionText { get; set; }
        public string CurrentAnswerA { get; set; }
        public string CurrentAnswerB { get; set; }
        public string CurrentAnswerC { get; set; }
        public string CurrentAnswerD { get; set; }
    }
}