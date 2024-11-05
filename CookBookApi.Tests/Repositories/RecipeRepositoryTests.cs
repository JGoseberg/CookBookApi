using AutoMapper;
using CookBookApi.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class RecipeRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<CookBookContext>()
            .UseInMemoryDatabase(databaseName: "CookBook")
            .Options;

        _mapper = MapperTestConfig.InitializeAutoMapper();
    }

    [TearDown]
    public void TearDown()
    {
        using var context = new CookBookContext(_options);
        context.Database.EnsureDeleted();
    }

    [Test]
    public void AddRecipeAsync_ValidRecipe_ShouldBeAddedInDatabase()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void AnyRecipesWithCuisineAsync_ValidCuisine_ShouldReturnTrue()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void AnyRecipesWithCuisineAsync_InvalidCuisine_ShouldReturnFalse()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void DeleteRecipeAsync_ValidRecipe_ShouldBeDeletedFromDatabase()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    public void GetAllRecipesAsync_ShouldReturnAllRecipesInDatabaseWithAllDetails()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void GetRecipeByIdAsync_ValidId_ShouldReturnRecipeWithDetails()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void GetRandomRecipeAsync_ShouldReturnRandomRecipe()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void GetRecipesWithSpecificCuisineAsync_ValidCuisine_ShouldReturnAllRecipesWithSpecificCuisine()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void GetRecipesWithRestrictionsAsync_ValidListOfRestrictionIds_ShouldReturnAllRecipesWithRestrictions()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void UpdateRecipeAsync_ValidRecipe_ShouldBeUpdatedInDatabase()
    {
        throw new NotImplementedException();
    }
}