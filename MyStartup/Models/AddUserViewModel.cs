using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class AddUserViewModel : EditUserViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Debes introducir un email válido.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [MinLength(6, ErrorMessage = "El campo {0} debe tener una longitud mínima de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Password { get; set; }

        [Display(Name = "Confirmación de contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "El campo {0} debe tener una longitud mínima de {1} carácteres.")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmacíón de contraseña no son iguales.")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Registrarse Como...")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un rol.")]
        public int RoleId { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
