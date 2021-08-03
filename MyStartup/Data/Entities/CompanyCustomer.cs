using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStartup.Data.Entities
{
    public class CompanyCustomer
    {
        public int Id { get; set; }

        public Company Company { get; set; }

        public Customer Customer { get; set; }

        

    }
}
