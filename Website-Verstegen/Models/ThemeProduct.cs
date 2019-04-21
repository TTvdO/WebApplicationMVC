namespace Website_Verstegen.Models
{
    public class ThemeProduct
    {
        public int ThemeProductId { get; set; }

        public int Id { get; set; }
        public virtual Product Product { get; set; }

        public int ThemeId { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
