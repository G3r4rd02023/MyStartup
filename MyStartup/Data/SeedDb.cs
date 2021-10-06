using MyStartup.Data.Entities;
using MyStartup.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStartup.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRoles();
            var manager = await CheckUserAsync("0801-1992-11010", "Gerardo", "Antonio", "glanza007@gmail.com", "33077964", "Calle El paraiso", "Manager");
            var owner = await CheckUserAsync("0801-1998-10201", "Maria Celeste", "Valle", "maria.celeste@gmail.com", "3534 2747", "Calle Luna Calle Sol", "Owner");
            var lessee = await CheckUserAsync("0801-1990-13030", "Carlos Antonio", "Montoya", "carlos.antony@gmail.com", "35032747", "Calle Luna Calle Sol", "Customer");          
            await CheckManagerAsync(manager);
            await CheckOwnersAsync(owner);
            await CheckCustomersAsync(lessee);
        }

        private async Task CheckManagerAsync(User user)
        {
            if (!_context.Managers.Any())
            {
                _context.Managers.Add(new Manager { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                AddCategory("Ropa");
                AddCategory("Tecnología");
                AddCategory("Mascotas");
                await _context.SaveChangesAsync();
            }
        }

        private void AddCategory(string name)
        {
            _context.Categories.Add(new Category { Name = name, ImageUrl = $"~/images/Categories/{name}.png" });
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Manager");
            await _userHelper.CheckRoleAsync("Owner");
            await _userHelper.CheckRoleAsync("Customer");
        }

        private async Task CheckCustomersAsync(User user)
        {
            if (!_context.Customers.Any())
            {
                _context.Customers.Add(new Customer { User = user });
                await _context.SaveChangesAsync();
            }
        }

        

        private async Task CheckOwnersAsync(User user)
        {
            if (!_context.Owners.Any())
            {
                _context.Owners.Add(new Owner { User = user });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Honduras",
                    Cities = new List<City>
                {
                    new City
                    {
                        Name = "Tegucigalpa",
                       
                    },
                    new City
                    {
                        Name = "San Pedro Sula",
                       
                    },
                    new City
                    {
                        Name = "La Ceiba",
                       
                    }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "USA",
                    Cities = new List<City>
                {
                    new City
                    {
                        Name = "Los Angeles",
                        
                    },
                    new City
                    {
                        Name = "New York",
                       
                    }
                }
                });
                await _context.SaveChangesAsync();
            }
        }
    }


}
