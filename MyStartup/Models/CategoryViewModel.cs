using Microsoft.AspNetCore.Http;
using MyStartup.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class CategoryViewModel : Category
    {
        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

    }
}
