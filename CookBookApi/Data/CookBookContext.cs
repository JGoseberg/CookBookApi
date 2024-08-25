using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

public class CookBookContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Subrecipe> Subrecipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    public CookBookContext(DbContextOptions<CookBookContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Subrecipes)
            .WithMany(r => r.ParentRecipes)
            .UsingEntity(j => j.ToTable("RecipeSubrecipes"));

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithOne(i => i.Recipe)
            .HasForeignKey(i => i.RecipeId);
    }
}