using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class RecipeRestrictionRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;

    private readonly RecipeRestriction _recipeRestriction = new RecipeRestriction
    {
        RecipeId = 1,
        RestrictionId = 1
    };
    
    [SetUp]
    public void SetUp()
    {
        _options = new DbContextOptionsBuilder<CookBookContext>()
            .UseInMemoryDatabase(databaseName: "CookBook")
            .Options;
    }

    [TearDown]
    public void TearDown()
    {
        using var context = new CookBookContext(_options);
        context.Database.EnsureDeleted();
    }

    [Test]
    public async Task AnyRecipeWithRestrictionAsync_RestrictionExists_ReturnsTrue()
    {
        await using var context = new CookBookContext(_options);

        await context.RecipeRestrictions.AddAsync(_recipeRestriction);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRestrictionRepository(context);
        
        var result = await repository.AnyRecipeWithRestrictionAsync(_recipeRestriction.RestrictionId);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyRecipeWithRestrictionAsync_RestrictionDoesNotExist_ReturnsFalse()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);

        await context.RecipeRestrictions.AddAsync(_recipeRestriction);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRestrictionRepository(context);
        
        var result = await repository.AnyRecipeWithRestrictionAsync(invalidId);
        
        Assert.That(result, Is.False);
    }
}