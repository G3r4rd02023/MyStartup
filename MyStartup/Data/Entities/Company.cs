using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Data.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [DisplayName("Empresa")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Logo")]
        public string ImageUrl { get; set; }

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
          ? null
            : $"https://mystoreweb.azurewebsites.net{ImageUrl.Substring(1)}";


        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<CompanyCustomer> CompanyCustomers { get; set; }

        public Owner Owner { get; set; }

    }
}
