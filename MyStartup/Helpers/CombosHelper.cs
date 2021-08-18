using Microsoft.AspNetCore.Mvc.Rendering;
using MyStartup.Data;
using System.Collections.Generic;
using System.Linq;

namespace MyStartup.Helpers
{
    public class CombosHelper:ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "(Selecciona un rol...)" },
                new SelectListItem { Value = "1", Text = "Cliente" },
                new SelectListItem { Value = "2", Text = "Emprendedor" }
            };

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            List<SelectListItem> list = _context.Categories.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una categoría...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCompanies()
        {
            List<SelectListItem> list = _context.Companies.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una empresa...]",
                Value = "0"
            });

            return list;
        }
        public IEnumerable<SelectListItem> GetComboProducts()
        {
            List<SelectListItem> list = _context.Products.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un producto...]",
                Value = "0"
            });

            return list;
        }

    }
}
