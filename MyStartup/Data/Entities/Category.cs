using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Categoria")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://myleasing.azurewebsites.net{ImageUrl[1..]}";
    }
}
