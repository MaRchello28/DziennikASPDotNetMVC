namespace DziennikASPDotNetMVC.Models
{
    public class ShowClassScheduleViewModel
    {
        public string StudentName { get; set; }
        public List<DaySchedule> Sessions { get; set; }
    }

    public class DaySchedule
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<SessionDetails> Sessions { get; set; }
    }

    public class SessionDetails
    {
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string TeacherSurname { get; set; }
        public string Room { get; set; }
        public TimeSpan HourFrom { get; set; }
        public TimeSpan HourTo { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }



}
