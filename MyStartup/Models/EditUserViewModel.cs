using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }      

        [Display(Name = "DNI")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public string ImageUrl { get; set; }

        // TODO: Change the path when publish
        [Display(Name = "Foto")]
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? $"https://localhost:44385/images/noimage.png"
            : $"https://localhost:44385{ImageUrl[1..]}";

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }

    }
}
