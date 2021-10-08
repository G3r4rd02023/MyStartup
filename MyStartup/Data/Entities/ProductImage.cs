using System.ComponentModel.DataAnnotations;

namespace MyStartup.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://mystoreweb.azurewebsites.net{ImageUrl.Substring(1)}";

        public Product Product { get; set; }
    }
}
