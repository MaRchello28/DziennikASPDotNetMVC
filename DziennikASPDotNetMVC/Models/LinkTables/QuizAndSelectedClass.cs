namespace DziennikASPDotNetMVC.Models.LinkTables
{
    public class QuizAndSelectedClass
    {
        public int Id { get; set; }
        public int quizId { get; set; }
        public Quiz quiz { get; set; }
        public int studentClassId { get; set; }
        public DateTime? availableFrom { get; set; }
        public DateTime? availableTo { get; set; }

        public QuizAndSelectedClass() { }
        public QuizAndSelectedClass(int quizId, int studentClassId, DateTime availableFrom, DateTime availableTo)
        {
            this.quizId = quizId;
            this.studentClassId = studentClassId;
            this.availableFrom = availableFrom;
            this.availableTo = availableTo;
        }
    }
}
