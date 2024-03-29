﻿using MyStartup.Data;
using MyStartup.Data.Entities;
using MyStartup.Models;
using System.Threading.Tasks;

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

        public async Task<Company> ToCompanyAsync(CompanyViewModel model, bool isNew, string path)
        {
            return new Company
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                Name = model.Name,
                Owner = await _context.Owners.FindAsync(model.OwnerId)
            };
        }

        public CompanyViewModel ToCompanyViewModel(Company company)
        {
            return new CompanyViewModel
            {
                Id = company.Id,
                ImageUrl = company.ImageUrl,
                Name = company.Name,
                Owner = company.Owner,
                OwnerId = company.Owner.Id
            };
        }

        public async Task<Product> ToProductAsync(ProductViewModel model, bool isNew,string path)
        {
            return new Product
            {
                Category = await _context.Categories.FindAsync(model.CategoryId),
                Company = await _context.Companies.FindAsync(model.CompanyId),
                Description = model.Description,
                Id = isNew ? 0 : model.Id,
                IsActive = model.IsActive,
                IsStarred = model.IsStarred,
                Name = model.Name,
                Price = model.Price,
                ProductImages = model.ProductImages
                
            };
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Categories = _combosHelper.GetComboCategories(),
                Category = product.Category,
                CategoryId = product.Category.Id,
                Companies = _combosHelper.GetComboCompanies(),
                Company = product.Company,
                CompanyId = product.Company.Id,
                Description = product.Description,
                Id = product.Id,
                IsActive = product.IsActive,
                IsStarred = product.IsStarred,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages
            };
        }

    }
}
