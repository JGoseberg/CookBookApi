using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Data
{
    public class CookBookContext : DbContext
    {
        public CookBookContext(DbContextOptions<CookBookContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<MeasurementUnit> MeasurementsUnits { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<CountryKitchen> CountryKitchens { get; set; }
        public DbSet<IngredientCountry> IngredientCountries { get; set; }
        public DbSet<RecipeRecipe> RecipeRecipes { get; set; }
        public DbSet<DietaryRestriction> DietaryRestrictions { get; set; }
        public DbSet<RecipeDietaryRestriction> RecipeDietaryRestrictions { get; set; }
        public DbSet<IngredientDietaryRestriction> IngredientDietaryRestrictions { get; set; }


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
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Measurement)
                .WithMany(m => m.RecipeIngredients)
                .HasForeignKey(ri => ri.MeasurementId);


            modelBuilder.Entity<Measurement>()
                .HasOne(m => m.MeasurementUnit)
                .WithMany(mu => mu.Measurements)
                .HasForeignKey(m => m.MeasurementUnitId);


            modelBuilder.Entity<IngredientCountry>()
                .HasKey(ic => new { ic.IngredientID, ic.CountryKitchenId });

            modelBuilder.Entity<IngredientCountry>()
                .HasOne(ic => ic.Ingredient)
                .WithMany(i => i.IngredientCountries)
                .HasForeignKey(ic => ic.IngredientID);

            modelBuilder.Entity<IngredientCountry>()
                .HasOne(ic => ic.CountryKitchen)
                .WithMany(i => i.IngredientCountries)
                .HasForeignKey(ic => ic.CountryKitchenId);


            modelBuilder.Entity<RecipeRecipe>()
                .HasKey(rr => new { rr.ParentRecipeId, rr.ChildRecipeId });

            modelBuilder.Entity<RecipeRecipe>()
                .HasOne(rr => rr.ParentRecipe)
                .WithMany(r => r.ChildRecipes)
                .HasForeignKey(rr => rr.ParentRecipeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeRecipe>()
                .HasOne(rr => rr.ChildRecipe)
                .WithMany(r => r.ParentRecipes)
                .HasForeignKey(rr => rr.ChildRecipeId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<RecipeDietaryRestriction>()
                .HasKey(rd => new { rd.RecipeId, rd.DietaryRecipeRestrictionId });

            modelBuilder.Entity<RecipeDietaryRestriction>()
                .HasOne(rd => rd.Recipe)
                .WithMany(r => r.RecipeDietaryRestrictions)
                .HasForeignKey(rd => rd.RecipeId);

            modelBuilder.Entity<RecipeDietaryRestriction>()
                .HasOne(rd => rd.DietaryRestriction)
                .WithMany(dr => dr.RecipeDietaryRestrictions)
                .HasForeignKey(rd => rd.DietaryRecipeRestrictionId);


            modelBuilder.Entity<IngredientDietaryRestriction>()
                .HasKey(id => new { id.IngredientsId, id.DietaryRestrictionId });

            modelBuilder.Entity<IngredientDietaryRestriction>()
                .HasOne(id => id.Ingredient)
                .WithMany(i => i.IngredientDietaryRestrictions)
                .HasForeignKey(id =>  id.IngredientsId);

            modelBuilder.Entity<IngredientDietaryRestriction>()
                .HasOne(id => id.DietaryRestriction)
                .WithMany(dr => dr.IngredientDietaryRestrictions)
                .HasForeignKey(id => id.DietaryRestrictionId);
        }
    }
}
