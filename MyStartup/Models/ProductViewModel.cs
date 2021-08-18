using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyStartup.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Empresa")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una empresa.")]
        [Required]
        public int CompanyId { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }

    }
}
