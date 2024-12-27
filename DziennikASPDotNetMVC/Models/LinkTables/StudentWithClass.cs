namespace DziennikASPDotNetMVC.Models.LinkTable
{
    public class StudentWithClass
    {
        public int id {  get; set; }
        public int studentId { get; set; }
        public int studentClassId { get; set; }
        public StudentWithClass() { }
        public StudentWithClass(int studentId, int studentClassId) 
        { 
            this.studentId = studentId;
            this.studentClassId = studentClassId;
        }
    }
}
