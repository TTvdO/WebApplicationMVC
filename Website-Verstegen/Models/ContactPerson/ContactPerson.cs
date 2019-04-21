using System;
using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class ContactPerson
    {
        [Key]
        public int Id { get; set; }
        public String Naam { get; set; }
        public String Functie { get; set; }
        public int Telefoonnummer { get; set; }
        public String EmailAdres { get; set; }
        public String ImageUrl { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}
