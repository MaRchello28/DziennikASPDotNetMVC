using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Attendance
    {
        public int attendanceId {  get; set; }
        public string isPresent {  get; set; }
        public int studentId { get; set; }
        public int lessonId { get; set; }
        public Attendance() { }
        public Attendance(string isPresent, int studentId, int lessonId) { 
            this.isPresent = isPresent; this.studentId = studentId; this.lessonId = lessonId;
        }
    }
}
