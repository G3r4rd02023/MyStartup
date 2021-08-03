using MyStartup.Data.Entities;
using MyStartup.Models;

namespace MyStartup.Helpers
{
    public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, bool isNew,string path);

        CategoryViewModel ToCategoryViewModel(Category category);

    }
}
