using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class User
    {
        public int userId {  get; set; }
        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string name { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string surname { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public virtual ICollection<Mail> messages { get; set; } = new List<Mail>();

        [RegularExpression("^(admin|student|teacher|parent)$", ErrorMessage = "Invalid user type.")]
        public string type { get; set; }
        public User() { }
        public User(string name, string surname, string login, string password, string type)
        {
            this.name = name; this.surname = surname; this.login = login; this.password = password;
            this.type = type;
        }
    }
}
