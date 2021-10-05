using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStartup.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [DisplayName("País")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
       
        [DisplayName("# Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
