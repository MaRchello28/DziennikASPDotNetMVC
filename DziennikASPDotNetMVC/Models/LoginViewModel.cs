using System.ComponentModel.DataAnnotations;

namespace DziennikASPDotNetMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login jest wymagany.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
