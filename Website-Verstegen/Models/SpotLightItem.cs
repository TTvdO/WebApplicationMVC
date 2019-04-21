namespace Website_Verstegen.Models
{
    //Kijk Recipe.cs -> Type word hardcoded meegegeven
    public class SpotLightItem
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string SpotlightUrl
        {
            get
            {
                if (this.Type == "Product" || this.Type == "Packaging")
                {
                    return this.Type + 's' + "/" + this.Type + "Details" + "/" + this.Id;
                }
                else
                {
                    return "/UnderConstruction";
                }
            }
        }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }
}

