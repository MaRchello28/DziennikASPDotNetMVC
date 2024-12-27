using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Session
    {
        public int sessionId { get; set; }
        public int subjectId { get; set; }
        public int teacherId { get; set; }
        public int studentClassId {  get; set; }
        public DayOfWeek dayOfTheWeek { get; set; }
        public TimeSpan hourFrom {  get; set; }
        public TimeSpan hourTo { get; set; }
        public virtual ICollection<Lesson> lessons { get; set; }
        public int sala { get; set; }
        public Session() { }
        public Session(int subjectId, int teacherId, DayOfWeek dayOfTheWeek, TimeSpan hourFrom, TimeSpan hourTo, int studentClassId, int sala) 
        { 
            this.subjectId = subjectId; this.teacherId = teacherId; this.dayOfTheWeek = dayOfTheWeek; 
            this.hourFrom = hourFrom; this.hourTo = hourTo; lessons = new List<Lesson>();
            this.sala = sala; this.studentClassId = studentClassId;
        }
        public Session(int subjectId, int teacherId, DayOfWeek dayOfTheWeek, TimeSpan hourFrom, TimeSpan hourTo, int sala)
        {
            this.subjectId = subjectId; this.teacherId = teacherId; this.dayOfTheWeek = dayOfTheWeek;
            this.hourFrom = hourFrom; this.hourTo = hourTo; lessons = new List<Lesson>();
            this.sala = sala;
        }
    }
}
