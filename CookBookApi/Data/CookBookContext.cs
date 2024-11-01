using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

public class CookBookContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Subrecipe> Subrecipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
    public DbSet<Cuisine> Cuisines { get; set; }
    public DbSet<Restriction> Restrictions { get; set; }
    public DbSet<RecipeRestriction> RecipeRestrictions { get; set; }
    public DbSet<IngredientRestriction> IngredientRestrictions { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<RecipeImage> RecipeImages { get; set; }

    public CookBookContext(DbContextOptions<CookBookContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeIngredients)
            .HasForeignKey(ri => ri.RecipeId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.MeasurementUnit)
            .WithMany()
            .HasForeignKey(ri => ri.MeasurementUnitId);


        // Configure Recipe-Subrecipe relationship (self-referencing)
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Subrecipes)
            .WithOne()
            .HasForeignKey(r => r.ParentRecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Ingredient-Cuisine relationship
        modelBuilder.Entity<Ingredient>()
            .HasOne(i => i.Cuisine)
            .WithMany(c => c.Ingredients)
            .HasForeignKey(i => i.CuisineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Recipe-Cuisine relationship
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Cuisine)
            .WithMany(c => c.Recipes)
            .HasForeignKey(r => r.CuisineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure many-to-many relationships for Restrictions
        modelBuilder.Entity<RecipeRestriction>()
            .HasKey(rr => new { rr.RecipeId, rr.RestrictionId });

        modelBuilder.Entity<RecipeRestriction>()
            .HasOne(rr => rr.Recipe)
            .WithMany(r => r.RecipeRestrictions)
            .HasForeignKey(rr => rr.RecipeId);

        modelBuilder.Entity<RecipeRestriction>()
            .HasOne(rr => rr.Restriction)
            .WithMany(r => r.RecipeRestrictions)
            .HasForeignKey(rr => rr.RestrictionId);

        modelBuilder.Entity<IngredientRestriction>()
            .HasKey(ir => new { ir.IngredientId, ir.RestrictionId });

        modelBuilder.Entity<IngredientRestriction>()
            .HasOne(ir => ir.Ingredient)
            .WithMany(i => i.IngredientRestrictions)
            .HasForeignKey(ir => ir.IngredientId);

        modelBuilder.Entity<IngredientRestriction>()
            .HasOne(ir => ir.Restriction)
            .WithMany(r => r.IngredientRestrictions)
            .HasForeignKey(ir => ir.RestrictionId);

        modelBuilder.Entity<Cuisine>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<MeasurementUnit>()
            .HasIndex(m => m.Name)
            .IsUnique();

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.RecipeImages)
            .WithOne(ri => ri.Recipe)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<RecipeImage>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.RecipeImages)
            .HasForeignKey(ri => ri.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}