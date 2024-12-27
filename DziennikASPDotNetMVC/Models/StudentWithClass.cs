using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class StudentWithClass
    {
        public int Id { get; set; }
        public int studentClassId { get; set; }
        public StudentClass StudentClass { get; set; }

        public int studentId { get; set; }
        public User Student { get; set; }
    }

}
