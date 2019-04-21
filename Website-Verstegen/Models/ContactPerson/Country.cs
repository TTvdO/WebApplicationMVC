using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Website_Verstegen.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public String Naam { get; set; }
        public String Code { get; set; }
        public ICollection<Province> Provinces { get; set; }
        public ICollection<ContactPerson> People { get; set; }
    }
}
