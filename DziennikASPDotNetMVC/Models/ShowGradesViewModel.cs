using System;
using System.Collections.Generic;

namespace DziennikASPDotNetMVC.Models
{
    public class ShowGradesViewModel
    {
        public List<User> Students { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<int> SelectedStudents { get; set; }
        public List<int> SelectedSubjects { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }
        public List<dynamic> GradesTable { get; set; }
    }
}
