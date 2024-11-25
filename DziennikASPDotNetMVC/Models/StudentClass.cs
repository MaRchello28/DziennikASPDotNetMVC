using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class StudentClass
    {
        public int studentClassId {  get; set; }
        [MaxLength(1)]
        public string number {  get; set; }
        [MaxLength(1)]
        public string letter { get; set; }
        public int teacherId { get; set; }
        public int? classScheduleId { get; set; }
        public StudentClass() { }
        public StudentClass(string number, string letter, int teacherId) 
        { 
            this.number = number; this.letter = letter; this.teacherId = teacherId;
        }
    }
}
