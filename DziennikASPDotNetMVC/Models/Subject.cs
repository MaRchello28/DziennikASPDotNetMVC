using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Subject
    {
        public int subjectId {  get; set; }
        public string name { get; set; }
        public Subject() { }
        public Subject(string name) 
        {
            this.name = name;
        }
    }
}
