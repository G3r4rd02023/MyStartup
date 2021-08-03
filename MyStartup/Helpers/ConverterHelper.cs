using MyStartup.Data;
using MyStartup.Data.Entities;
using MyStartup.Models;

namespace MyStartup.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public Category ToCategory(CategoryViewModel model, bool isNew,string path)
        {
            return new Category
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                Name = model.Name
            };
        }

        public CategoryViewModel ToCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                ImageUrl = category.ImageUrl,
                Name = category.Name
            };
        }
    }
}
