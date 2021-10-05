using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStartup.Data;
using MyStartup.Data.Entities;
using MyStartup.Helpers;
using MyStartup.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyStartup.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;

        public ManagersController(DataContext context, IUserHelper userHelper,IImageHelper imageHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
        }

        // GET: Managers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Managers
                .Include(m => m.User).ToListAsync());
        }

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            return View(new AddUserViewModel { RoleId = 3 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (view.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(view.ImageFile, "Users");
                }

                var user = await _userHelper.AddUser(view, "Manager",path);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(view);
                }

                var manager = new Manager { User = user };

                _context.Managers.Add(manager);
                await _context.SaveChangesAsync();                              
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = manager.User.Address,
                Document = manager.User.Document,
                FirstName = manager.User.FirstName,
                Id = manager.Id,
                LastName = manager.User.LastName,
                PhoneNumber = manager.User.PhoneNumber
            };

            return View(view);
        }


        // POST: Managers/Edit/5        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var manager = await _context.Managers
                    .Include(m => m.User)
                    .FirstOrDefaultAsync(o => o.Id == view.Id);

                manager.User.Document = view.Document;
                manager.User.FirstName = view.FirstName;
                manager.User.LastName = view.LastName;
                manager.User.Address = view.Address;
                manager.User.PhoneNumber = view.PhoneNumber;

                await _userHelper.UpdateUserAsync(manager.User);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(manager.User.Email);
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }
    }
}
