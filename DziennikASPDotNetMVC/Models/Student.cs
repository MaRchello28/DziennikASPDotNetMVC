using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Student
    {
        public int studentId { get; set; }
        public virtual User user { get; set; }
        public string name => user?.name;
        public string surname => user?.surname;
        public string login => user?.login;
        public string password => user?.password;
        public virtual ICollection<Grade> grades { get; set; }
        public int? studentClassId { get; set; }
        public int? parentId { get; set; }
        public Student() { }
        public Student(User user, int? studentClassId, int? studentParentId) 
        { 
            this.user = user; grades = new List<Grade>(); this.studentClassId = studentClassId; this.parentId = studentParentId;
        }
    }
}
