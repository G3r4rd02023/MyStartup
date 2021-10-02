using Microsoft.AspNetCore.Authorization;
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
   
    public class OwnersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public OwnersController(DataContext context, IUserHelper userHelper,
            ICombosHelper combosHelper,IConverterHelper converterHelper,IImageHelper imageHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Owners
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Manager"))
            {
                return View(await _context.Owners
                .Include(o => o.User)
               .ToListAsync());
            }

            return View(await _context.Owners
               .Include(o => o.User)
               .Where(x => x.User.UserName == User.Identity.Name)
              .ToListAsync());
         }       

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Companies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 2 };
            return View(view);
        }

        // POST: Owners/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(model);
                if (user != null)
                {
                    var owner = new Owner
                    {
                        Companies = new List<Company>(),
                        User = user
                    };
                    _context.Owners.Add(owner);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "User with this eamil already exists.");              
            }
            return View(model);
        }

        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                user = await _userHelper.GetUserByEmailAsync(model.Username);
                await _userHelper.AddUserToRoleAsync(user, "Owner");
                return user;
            }

            return null;
        }


        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (owner == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Address = owner.User.Address,
                Document = owner.User.Document,
                FirstName = owner.User.FirstName,
                Id = owner.Id,
                LastName = owner.User.LastName,
                PhoneNumber = owner.User.PhoneNumber
            };

            return View(model);
        }

        // POST: Owners/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var owner = await _context.Owners
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);

                owner.User.Document = model.Document;
                owner.User.FirstName = model.FirstName;
                owner.User.LastName = model.LastName;
                owner.User.Address = model.Address;
                owner.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(owner.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Companies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            if (owner.Companies.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "Owner can't be delete because it has companies.");
                return RedirectToAction(nameof(Index));
            }

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(owner.User.Email);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddCompany(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FindAsync(id);

            if (owner == null)
            {
                return NotFound();
            }

            var model = new CompanyViewModel
            {
                OwnerId = owner.Id,                
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile,"Companies");
                }
                try
                {
                    var company = await _converterHelper.ToCompanyAsync(model, true, path);
                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                    //return RedirectToAction("Details", new { id = company.Id });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }           
            return View(model);
        }

        public async Task<IActionResult> EditCompany(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Company company = await _context.Companies
                .Include(x => x.Owner)
                .Include(x => x.Products)                 
                .FirstOrDefaultAsync(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            CompanyViewModel model = _converterHelper.ToCompanyViewModel(company);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompany(int id, CompanyViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            string path = string.Empty;

            if (model.ImageFile != null)
            {
                path = await _imageHelper.UploadImageAsync(model.ImageFile, "Companies");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Company company = await _converterHelper.ToCompanyAsync(model, false, path);
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = model.OwnerId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una empresa con ese nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }                        
            return View(model);
        }

        public async Task<IActionResult> DetailsCompany(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(o => o.Owner)
                .Include(c => c.Products)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.Products)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                CompanyId = company.Id,
                Categories = _combosHelper.GetComboCategories(),
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var company = await _context.Companies
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(x => x.Id == model.CompanyId);

                if (company == null)
                {
                    return NotFound();
                }

                string path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Products");
                }

                Product product = await _converterHelper.ToProductAsync(model, true, path);

                if (product.ProductImages == null)
                {
                    product.ProductImages = new List<ProductImage>();
                }

                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = path
                });

                try
                {
                    company.Products.Add(product);
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsCompany), new { id = company.Id });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un producto con ese nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            model.Categories = _combosHelper.GetComboCategories();
            return View(model);
        }

        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(x => x.Company)               
                .Include(x => x.ProductImages)                
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> AddProductImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            AddProductImageViewModel model = new()
            {
                ProductId = product.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = await _imageHelper.UploadImageAsync(model.ImageFile,"Products");
                Product product = await _context.Products
                    .Include(x => x.ProductImages)
                    .FirstOrDefaultAsync(x => x.Id == model.ProductId);
                if (product.ProductImages == null)
                {
                    product.ProductImages = new List<ProductImage>();
                }

                product.ProductImages.Add(new ProductImage
                {
                    ImageUrl = path
                });

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsProduct), new { id = product.Id });
            }

            return View(model);

        }

    }
}
