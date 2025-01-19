namespace DziennikASPDotNetMVC.Models.LinkTables
{
    public class ParentWithKid
    {
        public int id {  get; set; }
        public int studentId { get; set; }
        public int parentId { get; set; }
        public ParentWithKid() { }
        public ParentWithKid(int s, int p)
        {
            this.studentId = s;
            this.parentId = p;
        }
    }
}
