using CookBookApi.Models;
using CookBookApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Tests.Repositories;

[TestFixture]
public class RecipeImageRepositoryTests
{
    private DbContextOptions<CookBookContext> _options;
    
    private static readonly string Base64String =
        "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==";

    private readonly RecipeImage _recipeImage = new RecipeImage
    {
        ImageData = Convert.FromBase64String(Base64String), RecipeId = 1, MimeType = "image/jpeg"
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
    public async Task AddRecipeImageAsync_ValidImage_RecipeImageIsAdded()
    {
        await using var context = new CookBookContext(_options);
        
        var repository = new RecipeImageRepository(context);

        await repository.AddRecipeImageAsync(_recipeImage);

        var addedImage = await context.RecipeImages.FirstOrDefaultAsync();

        Assert.Multiple(() =>
        {
            Assert.That(addedImage, Is.Not.Null);
            Assert.That(addedImage!.Id, Is.EqualTo(_recipeImage.Id));
        });
    }

    [Test]
    public async Task GetRecipeImageAsync_ValidRecipeId_ReturnsRecipeImage()
    {
        await using var context = new CookBookContext(_options);
        
        await context.RecipeImages.AddAsync(_recipeImage); 
        await context.SaveChangesAsync();
            
        var repository = new RecipeImageRepository(context); 
        var images = await repository.GetRecipeImagesAsync(_recipeImage.RecipeId);

        Assert.Multiple(() =>
        {
            var recipeImages = images.ToList();
            Assert.That(recipeImages, Is.Not.Empty);
            Assert.That(recipeImages.Count, Is.EqualTo(1));
            Assert.That(recipeImages.First().Id, Is.EqualTo(_recipeImage.Id));
        });
    }
    
    [Test]
    public async Task GetRecipeImageAsync_InvalidRecipeId_ReturnsEmpty()
    {
        var notExistingRecipeId = 404;

        await using var context = new CookBookContext(_options);
        
        await context.RecipeImages.AddAsync(_recipeImage); 
        await context.SaveChangesAsync();
            
        var repository = new RecipeImageRepository(context); 
        var images = await repository.GetRecipeImagesAsync(notExistingRecipeId);
            
        Assert.That(images, Is.Empty);
        
    }
    
    [Test]
    public async Task GetExistingImageAsync_RecipeImageExists_ReturnsExistingRecipeImage()
    {
        await using var context = new CookBookContext(_options);
        var repository = new RecipeImageRepository(context);
        
        await context.RecipeImages.AddAsync(_recipeImage);
        await context.SaveChangesAsync();
        
        var result = await repository.GetExistingImageAsync(_recipeImage.ImageData, _recipeImage.MimeType);
        
        Assert.That(result, Is.EqualTo(_recipeImage));
    }
    
    [Test]
    public async Task GetExistingImageAsync_RecipeImageDoesNotExist_ReturnsNull()
    {
        await using var context = new CookBookContext(_options);
        var repository = new RecipeImageRepository(context);

        var newImageData = new byte[] { 1, 2, 3 };
        
        await context.RecipeImages.AddAsync(_recipeImage);
        await context.SaveChangesAsync();
        
        var result = await repository.GetExistingImageAsync(newImageData, _recipeImage.MimeType);
        
        Assert.That(result, Is.Null);
    }
}