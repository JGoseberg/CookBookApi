using AutoMapper;
using CookBookApi.Mappings;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class IngredientRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    private IMapper _mapper;

    private readonly Ingredient _ingredient = new Ingredient
    {
        Id = 1,
        Name = "Foo",
    };
    
    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<CookBookContext>()
            .UseInMemoryDatabase(databaseName: "CookBookContext")
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
    public async Task AddIngredientAsync_AddsIngredient()
    {
        await using var context = new CookBookContext(_options);
        
        var repository = new IngredientRepository(context, _mapper);
        
        await repository.AddIngredientAsync(_ingredient);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.Ingredients.Count(), Is.EqualTo(1));
            Assert.That(context.Ingredients.First(), Is.EqualTo(_ingredient));
        });
    }

    [Test]
    public async Task AnyIngredientWithSameNameAsync_ReturnsTrue()
    {
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        var result = await repository.AnyIngredientWithSameNameAsync(_ingredient.Name);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task AnyIngredientWithSameNameAsync_ReturnsFalse()
    {
        var ingredient = new Ingredient
        {
            Name = "duplicate",
        };
        
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        var result = await repository.AnyIngredientWithSameNameAsync(ingredient.Name);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteIngredientAsync_ValidIngredientId_DeletesIngredient()
    {
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        await repository.DeleteIngredientAsync(_ingredient.Id);
        
        Assert.That(context.Ingredients.FirstOrDefault(i => i.Id == _ingredient.Id), Is.Null);
    }

    [Test]
    public async Task DeleteIngredientAsync_InValidIngredientId_ReturnsNull()
    {
        var ingredientId = 404;
        
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        await repository.DeleteIngredientAsync(ingredientId);
        
        Assert.That(context.Ingredients.FirstOrDefault(i => i.Id == _ingredient.Id), Is.EqualTo(_ingredient));
    }

    [Test]
    public async Task GetAllIngredientsAsync_ReturnsAllIngredients()
    {
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        var ingredients = await repository.GetAllIngredientsAsync();
        
        Assert.That(ingredients, Is.Not.Null);
        Assert.That(ingredients.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetIngredientByIdAsync_ValidIngredientId_ReturnsIngredient()
    {
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        var ingredient = await repository.GetIngredientByIdAsync(_ingredient.Id);
        
        Assert.That(ingredient, Is.Not.Null);
        Assert.That(ingredient, Is.EqualTo(ingredient));
    }

    [Test]
    public async Task GetIngredientByIdAsync_InvalidIngredientId_ReturnsNull()
    {
        var invalidIngredientId = 404;
        
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        var ingredient = await repository.GetIngredientByIdAsync(invalidIngredientId);
        
        Assert.That(ingredient, Is.Null);
    }

    [Test]
    public async Task UpdateIngredientAsync_ValidIngredient_UpdatesIngredient()
    {
        var validIngredient = new Ingredient
        {
            Id = 1,
            Name = "valid ingredient",
        };
        
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        await repository.UpdateIngredientAsync(validIngredient);
        
        Assert.That(context.Ingredients.FirstOrDefault(i => i.Name == validIngredient.Name), Is.Not.Null);
    }
    
    [Test]
    public async Task UpdateIngredientAsync_InvalidIngredient_DoesNotUpdatesIngredient()
    {
        var invalidIngredient = new Ingredient
        {
            Id = 404,
            Name = "invalid ingredient",
        };
        
        await using var context = new CookBookContext(_options);

        await context.Ingredients.AddAsync(_ingredient);
        await context.SaveChangesAsync();
        
        var repository = new IngredientRepository(context, _mapper);
        
        await repository.UpdateIngredientAsync(invalidIngredient);
        
        Assert.Multiple(() =>
        {
            Assert.That(context.Ingredients.FirstOrDefault(i => i.Name == invalidIngredient.Name), Is.Null);
            Assert.That(context.Ingredients.FirstOrDefault(i => i.Name == _ingredient.Name), Is.Not.Null);
        });
    }
}