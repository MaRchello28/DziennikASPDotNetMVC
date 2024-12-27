namespace DziennikASPDotNetMVC.Models
{
    public class HoursForLessons
    {
        public int id { get; set; }
        public TimeSpan hourFrom { get; set; }
        public TimeSpan hourTo { get; set; }
        public HoursForLessons() { }
        public HoursForLessons(TimeSpan hourFrom, TimeSpan hourTo) 
        { 
            this.hourFrom = hourFrom;
            this.hourTo = hourTo;
        }
    }
}
