using System.Collections.Generic;

namespace Website_Verstegen.Models
{
    public class Province
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<ContactPerson> People { get; set; }
    }
}
