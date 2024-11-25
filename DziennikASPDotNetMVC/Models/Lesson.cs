using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Lesson
    {
        public int lessonId {  get; set; }
        public int nuberOfLesson { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public virtual ICollection<Attendance> attendances { get; set; }
        public int sessionId { get; set; }
        public Lesson() { }
        public Lesson(string name, string description, int numberOfLesson, int sessionId) 
        { 
            this.name = name; this.description = description;
            attendances = new List<Attendance>(); this.nuberOfLesson=numberOfLesson;
            this.sessionId = sessionId; date = DateTime.Now;
        }
    }
}
