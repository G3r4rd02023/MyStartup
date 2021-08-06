using System;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class DeliverViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de Entrega")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }
    }
}
