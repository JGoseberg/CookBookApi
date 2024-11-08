using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class IngredientRestrictionRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;

    private readonly IngredientRestriction _ingredientRestriction = new IngredientRestriction
    {
        IngredientId = 1,
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
    public async Task AnyIngredientWithRestrictionAsync_RestrictionExists_ReturnsTrue()
    {
        await using var context = new CookBookContext(_options);

        await context.IngredientRestrictions.AddAsync(_ingredientRestriction);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRestrictionRepository(context);
        
        var result = await repository.AnyIngredientWithRestrictionAsync(_ingredientRestriction.RestrictionId);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyIngredientWithRestrictionAsync_RestrictionDoesNotExist_ReturnsFalse()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);

        await context.IngredientRestrictions.AddAsync(_ingredientRestriction);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRestrictionRepository(context);
        
        var result = await repository.AnyIngredientWithRestrictionAsync(invalidId);
        
        Assert.That(result, Is.False);
    }
}