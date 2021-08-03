using System.Collections.Generic;

namespace MyStartup.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<CompanyCustomer> CompanyCustomers { get; set; }
    }
}
