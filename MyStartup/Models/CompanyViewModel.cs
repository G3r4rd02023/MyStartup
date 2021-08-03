using Microsoft.AspNetCore.Http;
using MyStartup.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class CompanyViewModel: Company
    {
        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }
    }
}
