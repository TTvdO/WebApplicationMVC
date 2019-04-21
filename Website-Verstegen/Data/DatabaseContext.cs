using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Website_Verstegen.Models;

namespace Website_Verstegen.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactPerson>()
            .HasOne<Country>(s => s.Country)
            .WithMany(cp => cp.People)
            .HasForeignKey(s => s.CountryId);

            modelBuilder.Entity<ContactPerson>()
            .HasOne<Province>(s => s.Province)
            .WithMany(cp => cp.People)
            .HasForeignKey(s => s.ProvinceId);

            modelBuilder.Entity<Province>()
            .HasOne<Country>(c => c.Country)
            .WithMany(p => p.Provinces)
            .HasForeignKey(c => c.CountryId);

            //Begin many to many
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                 .HasOne<Recipe>(ri => ri.Recipe)
                 .WithMany(r => r.RecipeIngredients)
                 .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
             .HasOne<Ingredient>(ri => ri.Ingredient)
             .WithMany(i => i.RecipeIngredients)
             .HasForeignKey(ri => ri.IngredientId);
            //End many to many

            modelBuilder.Entity<PreparationStep>()
             .HasOne<Recipe>(ri => ri.Recipe)
             .WithMany(p => p.PreparationSteps)
             .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne<Recipe>(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<PreparationStep>()
                .Property(a => a.PreparationStepId).ValueGeneratedOnAdd();

            modelBuilder.Entity<RecipeIngredient>()
                .Property(a => a.RecipeIngredientId).ValueGeneratedOnAdd();

            //Product one to many ProductDetails
            modelBuilder.Entity<ProductDetails>()
                .HasOne<Product>(p => p.Product)
                .WithMany(pd => pd.ProductDetails)
                .HasForeignKey(p => p.ProductId);

            //Many-to-Many tussen Themes en Products
            modelBuilder.Entity<ThemeProduct>()
                .HasKey(tp => tp.ThemeProductId);

            modelBuilder.Entity<ThemeProduct>()
                .HasOne(tp => tp.Product)
                .WithMany(p => p.ThemeProducts)
                .HasForeignKey(tp => tp.Id);

            modelBuilder.Entity<ThemeProduct>()
                .HasOne(tp => tp.Theme)
                .WithMany(t => t.ThemeProducts)
                .HasForeignKey(tp => tp.ThemeId);

            //One-to-Many tussen Story en Themes
            modelBuilder.Entity<Story>()
                .HasOne(s => s.Theme)
                .WithMany(t => t.Stories)
                .HasForeignKey(s => s.ThemeId);
        }

        //List all models here
        public DbSet<ContactPerson> People { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Counties { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<PreparationStep> PreparationStep { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }

        public DbSet<Packaging> Packagings { get; set; }

        public DbSet<SpotLightConnector> SpotLightConnectors {get; set;}
        public DbSet<SpotlightContent> SpotlightContents { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Theme> Themes { get; set; }
        public DbSet<ThemeProduct> ThemeProducts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Story> Stories { get; set; }
    }
}
