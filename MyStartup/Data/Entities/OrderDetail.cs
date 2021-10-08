using System.ComponentModel.DataAnnotations;

namespace MyStartup.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Display(Name = "Precio")]      
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        [Display(Name = "Valor")]
        
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Value { get { return this.Price * (decimal)this.Quantity; } }
    }
}
