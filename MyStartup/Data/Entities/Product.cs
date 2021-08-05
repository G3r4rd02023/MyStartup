using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyStartup.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [DisplayName("Producto")]
        public string Name { get; set; }

        [DisplayName("Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Precio")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayName("Esta Activo")]
        public bool IsActive { get; set; }

        [DisplayName("Esta disponible")]
        public bool IsStarred { get; set; }

        public Category Category { get; set; }

        public Company Company { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        public string FirstImage
        {
            get
            {
                if (ProductImages == null || ProductImages.Count == 0)
                {
                    return "https://localhost:44385/wwwroot/images/App/noImage.png";
                }

                return ProductImages.FirstOrDefault().ImageFullPath;
            }
        }

    }
}
