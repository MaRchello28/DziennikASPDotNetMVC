namespace DziennikASPDotNetMVC.Models
{
    public class Inquiry
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string from { get; set; }
        public int toTeacherId { get; set; }
        public DateTime send { get; set; }
        public Inquiry() { }
        public Inquiry(string subject, string body, string from, int toTeacherId)
        {
            this.subject = subject; this.from = from; this.toTeacherId = toTeacherId; this.body = body;
        }
    }
}
