using System.ComponentModel.DataAnnotations;

namespace WebServerTest.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Show Password")]
        public bool ShowPassword { get; set; }

        // This field should not be validated
        [Display(Name = "Return URL")]
        public string ReturnUrl { get; set; } = string.Empty;
    }
} 