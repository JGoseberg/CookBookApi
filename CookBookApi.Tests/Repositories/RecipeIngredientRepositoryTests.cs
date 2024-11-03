using AutoMapper;
using CookBookApi.Mappings;
using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class RecipeIngredientRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    private readonly RecipeIngredient _recipeIngredient = new RecipeIngredient
    {
        RecipeId = 1,
        IngredientId = 1,
    };

    private readonly Ingredient _ingredient = new Ingredient
    {
        Name = "Foo",
        Id = 1
    };

    [SetUp]
    public void SetUp()
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
    public async Task GetRecipesWithIngredient_ValidIngredientId_ShouldReturnRecipeIds()
    {
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);

        var recipeIds = await repository.GetRecipesWithIngredientAsync(_ingredient.Id);
        
        Assert.That(recipeIds.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetRecipesWithIngredient_InvalidIngredientId_ShouldReturnNull()
    {
        var invalidIngredientId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);

        var recipeIds = await repository.GetRecipesWithIngredientAsync(invalidIngredientId);
        
        Assert.That(recipeIds, Is.Null);
    }

    [Test]
    public async Task GetRecipesWithIngredientsAsync_EmptyListOfIngredients_ShouldReturnNull()
    {
        var emptyListOfIngredientIds = new List<int>();
        
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);

        var recipeIds = await repository.GetRecipesWithIngredientsAsync(emptyListOfIngredientIds);
        
        Assert.That(recipeIds, Is.Null);
    }
    
    [Test]
    public async Task GetRecipesWithIngredientsAsync_ListWithOnlyOneIngredientId_ShouldReturnNull()
    {
        var listWithOnlyOneIngredientId = new List<int>
        {
            { 1 }
        };
        
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);

        var recipeIds = await repository.GetRecipesWithIngredientsAsync(listWithOnlyOneIngredientId);
        
        Assert.That(recipeIds, Is.Null);
    }
    
    [Test]
    public async Task GetRecipesWithIngredientsAsync_ValidListOfIngredientIds_ShouldReturnListOfRecipeIds()
    {
        var secondIngredient = new Ingredient
        {
            Name = "Bar",
            Id = 2
        };
        
        var listOfIngredientIds = new List<int>
        {
            { _ingredient.Id },
            { secondIngredient.Id }
        };

        var secondRecipeIngredient = new RecipeIngredient
        {
            Id = 2,
            RecipeId = 1,
            IngredientId = 2,
        };

        var recipe = new Recipe
        {
            Name = "Foo",
            Creator = "RecipeIngredientRepositoryTests",
            Description = "Foo",
            Instruction = "Bar"
        };
        
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.RecipeIngredients.AddAsync(secondRecipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.Ingredients.AddAsync(secondIngredient);
        await context.Recipes.AddAsync(recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);

        var recipeIds = await repository.GetRecipesWithIngredientsAsync(listOfIngredientIds);
        
        Assert.That(recipeIds.Count(), Is.EqualTo(1));
    }
    
    [Test]
    public async Task AnyRecipeWithIngredientAsync_IngredientExists_ShouldReturnTrue()
    {
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);
        
        var result = await repository.AnyRecipesWithIngredientAsync(_ingredient.Id);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task AnyRecipeWithMeasurementUnitAsync_MeasurementUnitDoesNotExists_ShouldReturnFalse()
    {
        var invalidIngredientId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.RecipeIngredients.AddAsync(_recipeIngredient);
        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new RecipeIngredientRepository(context, _mapper);
        
        var result = await repository.AnyRecipesWithIngredientAsync(invalidIngredientId);
        
        Assert.That(result, Is.False);
    }
}