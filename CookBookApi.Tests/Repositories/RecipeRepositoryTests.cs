using AutoMapper;
using CookBookApi.Mappings;
using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class RecipeRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    private readonly Recipe _recipe = new Recipe
    {
        Name = "Foo",
        Description = "Bar",
        Creator = "FooBar",
        Instruction = "BarFoo",
        Cuisine = new Cuisine
        {
            Name = "Foo",
        }
    };
    
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
    public async Task AddRecipeAsync_ValidRecipe_ShouldBeAddedInDatabase()
    {
        await using var context = new CookBookContext(_options);
        
        var repository = new RecipeRepository(context, _mapper);
        
        await repository.AddRecipeAsync(_recipe);
        
        var addedRecipe = context.Recipes.FirstOrDefault();
        
        Assert.Multiple(() =>
        {
            Assert.That(addedRecipe, Is.Not.Null);
            Assert.That(addedRecipe!.Name, Is.EqualTo(_recipe.Name));
        });
    }

    [Test]
    public async Task AnyRecipesWithCuisineAsync_ValidCuisine_ShouldReturnTrue()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var result = await repository.AnyRecipesWithCuisineAsync(_recipe.Cuisine.Id);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AnyRecipesWithCuisineAsync_InvalidCuisine_ShouldReturnFalse()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var result = await repository.AnyRecipesWithCuisineAsync(invalidId);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteRecipeAsync_ValidRecipe_ShouldBeDeletedFromDatabase()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        await repository.DeleteRecipeAsync(_recipe.Id);
        
        Assert.That(context.Recipes.FirstOrDefault(r => r.Id == _recipe.Id), Is.Null);
    }
    
    [Test]
    public async Task DeleteRecipeAsync_InvalidRecipe_ShouldNotDeleteFromDatabase()
    {
        var invalidId = 404;

        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        await repository.DeleteRecipeAsync(invalidId);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.Recipes.FirstOrDefault(r => r.Id == _recipe.Id), Is.Not.Null);
            Assert.That(context.Recipes.FirstOrDefault(r => r.Id == _recipe.Id), Is.EqualTo(_recipe));
        } );
    }
    
    [Test]
    public async Task GetAllRecipesAsync_ShouldReturnAllRecipesInDatabase()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var recipes = await repository.GetAllRecipesAsync();
        
        Assert.That(recipes.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetRecipeByIdAsync_ValidId_ShouldReturnRecipe()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var recipe = await repository.GetRecipeByIdAsync(_recipe.Id);
        
        Assert.Multiple(() =>
        {
            Assert.That(recipe!.Id, Is.EqualTo(_recipe.Id));
            Assert.That(recipe.Description, Is.EqualTo(_recipe.Description));
            Assert.That(recipe.Instruction, Is.EqualTo(_recipe.Instruction));
        });
    }
    
    [Test]
    public async Task GetRecipeByIdAsync_InvalidId_ShouldReturnNull()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var recipe = await repository.GetRecipeByIdAsync(invalidId);
        
        Assert.That(recipe, Is.Null);
    }

    [Test]
    public async Task GetRecipesWithSpecificCuisineAsync_ValidCuisine_ShouldReturnAllRecipesWithSpecificCuisine()
    {
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var recipes = await repository.GetRecipesWithSpecificCuisineAsync(_recipe.Cuisine.Id);
        
        Assert.That(recipes.FirstOrDefault(r => r!.Id == _recipe.Id)?.Name, Is.EqualTo(_recipe.Name));
    }

    [Test]
    public async Task GetRecipesWithSpecificCuisineAsync_InvalidCuisine_ShouldReturnEmptyCollection()
    {
        var invalidId = 404;
        
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        var recipes = await repository.GetRecipesWithSpecificCuisineAsync(invalidId);
        
        Assert.That(recipes.FirstOrDefault(), Is.Null);
    }
    
    [Test]
    public async Task UpdateRecipeAsync_ValidRecipe_ShouldBeUpdatedInDatabase()
    {
        var validRecipe = _recipe;
        validRecipe.Name = "validRecipe";
        
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        await repository.UpdateRecipeAsync(_recipe);
        
        Assert.That(context.Recipes.First().Name, Is.EqualTo(validRecipe.Name));
    }
    
    [Test]
    public async Task UpdateRecipeAsync_InvalidRecipe_ShouldNotUpdateInDatabase()
    {
        var invalidRecipe = new Recipe
        {
            Id = _recipe.Id,
        };
        
        await using var context = new CookBookContext(_options);
        
        await context.Recipes.AddAsync(_recipe);
        await context.SaveChangesAsync();
        
        var repository = new RecipeRepository(context, _mapper);
        
        await repository.UpdateRecipeAsync(_recipe);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.Recipes.FirstOrDefault(r => r.Name == invalidRecipe.Name), Is.Null);
            Assert.That(context.Recipes.FirstOrDefault(r => r.Name == _recipe.Name), Is.Not.Null);
        });
    }
}