using System.ComponentModel.DataAnnotations;

namespace DziennikASPDotNetMVC.Models.ResetPasswordModels
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Login is required.")]
        public string login { get; set; }
    }
}
