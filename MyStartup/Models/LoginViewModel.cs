using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordar Contraseña")]
        public bool RememberMe { get; set; }

    }
}
