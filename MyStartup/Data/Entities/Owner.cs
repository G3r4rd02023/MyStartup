using System.Collections.Generic;

namespace MyStartup.Data.Entities
{
    public class Owner
    {
        public int Id { get; set; }

        public User User { get; set; }

        public ICollection<Company> Companies { get; set; }

    }
}
