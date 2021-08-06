using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MyStartup.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboRoles();

        IEnumerable<SelectListItem> GetComboCategories();

        IEnumerable<SelectListItem> GetComboProducts();
    }
}
