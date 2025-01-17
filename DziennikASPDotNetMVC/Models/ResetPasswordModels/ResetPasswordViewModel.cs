namespace DziennikASPDotNetMVC.Models.ResetPasswordModels
{
    public class ResetPasswordViewModel
    {
        public string token { get; set; }
        public string login { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }
    }
}
