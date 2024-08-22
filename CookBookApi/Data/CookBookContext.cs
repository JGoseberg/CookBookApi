using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Data
{
    public class CookBookContext : DbContext 
    {
        public CookBookContext(DbContextOptions<CookBookContext> options)
            : base (options)
        {
        }

        public DbSet<Amount> Amounts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeSubRecipe> RecipeSubRecipes { get; set; }
        public DbSet<SubRecipe> SubRecipes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);



            modelBuilder.Entity<RecipeSubRecipe>()
                .HasKey(rr => new { rr.RecipeId, rr.SubRecipeId });

            modelBuilder.Entity<RecipeSubRecipe>()
                .HasOne(rr => rr.Recipe)
                .WithMany(r => r.RecipeSubRecipes)
                .HasForeignKey(rr => rr.RecipeId);

            modelBuilder.Entity<RecipeSubRecipe>()
                .HasOne(rr => rr.SubRecipe)
                .WithMany(r => r.RecipeSubRecipes)
                .HasForeignKey(rr => rr.SubRecipeId);



            //modelBuilder.Entity<Amount>().ToTable(nameof(Amount));
            //modelBuilder.Entity<Country>().ToTable(nameof(Country));
            //modelBuilder.Entity<Ingredient>().ToTable(nameof(Ingredient));
            //modelBuilder.Entity<Recipe>().ToTable(nameof(Recipe));
            //modelBuilder.Entity<SubRecipe>().ToTable(nameof(SubRecipe));      

        }
    }
}
