﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un producto.")]
        public int ProductId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "La cantidad debe ser un numero positivo")]
        public double Quantity { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; }
    }
}