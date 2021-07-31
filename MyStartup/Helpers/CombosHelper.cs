using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MyStartup.Helpers
{
    public class CombosHelper:ICombosHelper
    {     

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
    }
}
