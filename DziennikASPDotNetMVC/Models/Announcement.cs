using System.Drawing;

namespace DziennikASPDotNetMVC.Models
{
    public class Announcement
    {
        public int id {  get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public byte[]? image { get; set; }
        public DateTime whenUpload { get; set; }
        public int teacherId { get; set; }
        public Announcement() { }
        public Announcement(string title, string description, byte[] image, int teacherid)
        {
            this.title= title;
            this.description= description;
            this.image= image;
            this.whenUpload= DateTime.Now;
            this.teacherId= teacherid;
        }

        public Announcement(string title, string description, int teacherid)
        {
            this.title = title;
            this.description = description;
            this.whenUpload = DateTime.Now;
            this.teacherId = teacherid;
        }

        public static byte[] ConvertImageToByteArray(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        public static System.Drawing.Image ConvertByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }
    }
}
