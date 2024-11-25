using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Grade
    {
        public int gradeId {  get; set; }
        public double value { get; set; }
        public int wage {  get; set; }
        public string description { get; set; }
        public int studentId { get; set; }
        public int teacherId { get; set; }
        public DateTime time { get; set; }
        public int sessionId { get; set; }
        public int subjectId { get; set; }
        public Grade() { }
        public Grade(double value, int wage, string description, int studentId, int teacherId, int sessionId, int subjectId) 
        { 
            time = DateTime.Now; this.value=value; this.wage=wage; this.description=description; this.studentId=studentId; this.teacherId=teacherId; 
            this.sessionId=sessionId; this.subjectId=subjectId;
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
}
