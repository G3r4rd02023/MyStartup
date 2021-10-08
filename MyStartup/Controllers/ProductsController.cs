using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStartup.Data;
using MyStartup.Data.Entities;
using MyStartup.Helpers;
using MyStartup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStartup.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(DataContext context, IImageHelper imageHelper,
            ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Company)
                .Include(p => p.ProductImages)
                .ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(c => c.Category)
                .Include(p => p.Company)
                .Include(c => c.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


    }  
}
