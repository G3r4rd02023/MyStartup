using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class AddProductImageViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Imagen")]
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
