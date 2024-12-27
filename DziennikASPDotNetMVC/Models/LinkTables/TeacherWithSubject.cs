namespace DziennikASPDotNetMVC.Models.LinkTable
{
    public class TeacherWithSubject
    {
        public int id {  get; set; }
        public int teacherId { get; set; }
        public int subjectId { get; set; }
        public TeacherWithSubject() { }
        public TeacherWithSubject(int teacherId, int subjectId)
        { 
            this.teacherId = teacherId;
            this.subjectId = subjectId;
        }
    }
}
