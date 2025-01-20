namespace DziennikASPDotNetMVC.Models
{
    public class Inquiry
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public DateTime send { get; set; }
        public Inquiry() { }
        public Inquiry(string subject, string body, string from, string to)
        {
            this.subject = subject; this.from = from; this.to = to; this.body = body;
        }
    }
}
