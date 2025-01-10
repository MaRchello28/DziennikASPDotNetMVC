namespace DziennikASPDotNetMVC.Models.ForControllersOnly
{
    public class StudentViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public bool IsAssignedToClass { get; set; }
    }
}
