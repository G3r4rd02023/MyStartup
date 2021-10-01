using Microsoft.AspNetCore.Http;
using MyStartup.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class CompanyViewModel: Company
    {
        public int OwnerId { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }
    }
}
