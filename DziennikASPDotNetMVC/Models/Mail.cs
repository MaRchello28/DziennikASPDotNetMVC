using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class Mail
    {
        public int mailId {  get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string from { get; set; }
        public int toClassId { get; set; }
        public DateTime send {  get; set; }
        public Mail() { }
        public Mail(string subject, string body, string from, int toClassId) 
        { 
            this.subject = subject; this.from = from; this.toClassId = toClassId; this.body = body;
        }
    }
}
