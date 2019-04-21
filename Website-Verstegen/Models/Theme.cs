using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Website_Verstegen.Models
{
    public class Theme
    {
        public int ThemeId { get; set; }

        [Required]
        [StringLength(20)]
        public string ThemeName { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual List<ThemeProduct> ThemeProducts { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<Story> Stories { get; set; }
    }
}
