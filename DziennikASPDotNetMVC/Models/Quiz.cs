namespace DziennikASPDotNetMVC.Models
{
    public class Quiz
    {
        public int quizId { get; set; }
        public string name { get; set; }
        public TimeSpan? timeToWriteQuiz { get; set; }
        public int maxPoints { get; set; }
        public bool generateGrade { get; set; }
        public int teacherId { get; set; }
        public int subjectId {  get; set; }
        public Quiz() { }
        public Quiz(string name, TimeSpan timeToWriteQuiz, bool generateGrade, int teacherid, int subjectId) 
        { 
            this.name = name;
            this.timeToWriteQuiz = timeToWriteQuiz;
            maxPoints = 0;
            this.generateGrade = generateGrade;
            this.teacherId = teacherid;
            this.subjectId = subjectId;
        }
    }
}
