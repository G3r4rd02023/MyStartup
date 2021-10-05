using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyStartup.Data;
using MyStartup.Data.Entities;
using MyStartup.Models;
using System.Threading.Tasks;

namespace MyStartup.Helpers
{

    public class UserHelper : IUserHelper
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly SignInManager<User> _signInManager;
            private readonly DataContext _context;

            public UserHelper(
                    UserManager<User> userManager,
                    RoleManager<IdentityRole> roleManager,
                    SignInManager<User> signInManager,
                    DataContext context)
                {
                    _userManager = userManager;
                    _roleManager = roleManager;
                    _signInManager = signInManager;
                    _context = context;
                }


            public async Task<SignInResult> LoginAsync(LoginViewModel model)
            {
                return await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    model.RememberMe,
                    false);
            }

            public async Task LogoutAsync()
            {
                await _signInManager.SignOutAsync();
            }

            public async Task<IdentityResult> AddUserAsync(User user, string password)
            {
                return await _userManager.CreateAsync(user, password);
            }

            public async Task AddUserToRoleAsync(User user, string roleName)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }

            public async Task CheckRoleAsync(string roleName)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = roleName
                    });
                }
            }

            public async Task<User> GetUserByEmailAsync(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user;
            }

            public async Task<bool> IsUserInRoleAsync(User user, string roleName)
            {
                return await _userManager.IsInRoleAsync(user, roleName);
            }

            public async Task<IdentityResult> UpdateUserAsync(User user)
            {
                return await _userManager.UpdateAsync(user);
            }

        public async Task<User> AddUser(AddUserViewModel view, string role,string path)
        {
            var user = new User
            {
                Address = view.Address,
                Document = view.Document,
                Email = view.Username,
                FirstName = view.FirstName,
                LastName = view.LastName,
                PhoneNumber = view.PhoneNumber,
                UserName = view.Username,
                ImageUrl = path,
            };

            var result = await AddUserAsync(user, view.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            var newUser = await GetUserByEmailAsync(view.Username);
            await AddUserToRoleAsync(newUser, role);
            return newUser;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users
                .Include(u => u.City)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return true;
            }

            var response = await _userManager.DeleteAsync(user);
            return response.Succeeded;
        }
    }
}
