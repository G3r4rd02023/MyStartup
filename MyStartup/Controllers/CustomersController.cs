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
    public class CustomersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public CustomersController(DataContext context,IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers
                  .Include(o => o.User).
                  ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 1 };
            return View(view);
        }

        // POST: Customers/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.AddUser(view, "Customer");
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(view);
                }

                var customer = new Customer
                {
                    User = user
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                 .Include(o => o.User)
                 .FirstOrDefaultAsync(o => o.Id == id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = customer.User.Address,
                Document = customer.User.Document,
                FirstName = customer.User.FirstName,
                Id = customer.Id,
                LastName = customer.User.LastName,
                PhoneNumber = customer.User.PhoneNumber
            };

            return View(view);
        }

        // POST: Customers/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == view.Id);

                customer.User.Document = view.Document;
                customer.User.FirstName = view.FirstName;
                customer.User.LastName = view.LastName;
                customer.User.Address = view.Address;
                customer.User.PhoneNumber = view.PhoneNumber;

                await _userHelper.UpdateUserAsync(customer.User);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(customer.User.Email);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
