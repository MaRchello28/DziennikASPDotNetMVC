using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Parent
    {
        public int parentId {  get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public User user { get; set; }
        public string name => user?.name;
        public string surname => user?.surname;
        public string login => user?.login;
        public string password => user?.password;
        public virtual ICollection<Student> children { get; set; }
        public Parent() { }
        public Parent(string email, string phoneNumber, User user)
        {
            this.email = email; this.phoneNumber = phoneNumber;
            this.user = user;
        }
    }
}
