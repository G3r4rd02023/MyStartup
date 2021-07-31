using Microsoft.AspNetCore.Mvc;
using MyStartup.Data;
using MyStartup.Data.Entities;
using MyStartup.Helpers;
using MyStartup.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyStartup.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        public AccountController(IUserHelper userHelper,DataContext dataContext, ICombosHelper combosHelper)
        {
            _userHelper = userHelper;
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var view = new AddUserViewModel
            {
                Roles = _combosHelper.GetComboRoles()
            };

            return View(view);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var role = "Owner";
                if (view.RoleId == 1)
                {
                    role = "Customer";
                }

                var user = await _userHelper.AddUser(view, role);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(view);
                }

                if (view.RoleId == 1)
                {
                    var customer = new Customer
                    {                       
                        User = user
                    };

                    _dataContext.Customers.Add(customer);
                    await _dataContext.SaveChangesAsync();
                }
                else
                {
                    var owner = new Owner
                    {                       
                        User = user
                    };

                    _dataContext.Owners.Add(owner);
                    await _dataContext.SaveChangesAsync();
                }

                var loginViewModel = new LoginViewModel
                {
                    Password = view.Password,
                    RememberMe = false,
                    Username = view.Username
                };

                var result2 = await _userHelper.LoginAsync(loginViewModel);

                if (result2.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(view);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = user.Address,
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                user.Document = view.Document;
                user.FirstName = view.FirstName;
                user.LastName = view.LastName;
                user.Address = view.Address;
                user.PhoneNumber = view.PhoneNumber;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View(view);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }


    }
}
