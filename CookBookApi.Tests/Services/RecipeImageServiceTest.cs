using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using CookBookApi.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CookBookApi.Tests.Services;

[TestFixture]
public class RecipeImageServiceTest
{
    private RecipeImageService _recipeImageService;
    private Mock<IRecipeImageRepository> _recipeImageRepositoryMock;
    
    private const long BigFileSize = 6 * 1024 * 1024; // MB * KB * B
    private const long NormalFileSize = 1 * 1024 * 1024; // MB * KB * B

    [SetUp]
    public void Setup()
    {
        _recipeImageRepositoryMock = new Mock<IRecipeImageRepository>();
        _recipeImageService = new RecipeImageService(_recipeImageRepositoryMock.Object);
    }

    [Test]
    public async Task ProcessAndCreateRecipeImage_FileIsNull_ReturnsNull()
    {
        IFormFile? file = null;

        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file!);
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ProcessAndCreateRecipeImage_FileIsEmpty_ReturnsNull()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(0);
        
        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object);
        
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task ProcessAndCreateRecipeImage_FileIsToBig_ReturnsNull()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(BigFileSize);

        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object);
        
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task ProcessAndCreateRecipeImage_InvalidMimeType_ReturnsNull()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(NormalFileSize);
        file.Setup(f => f.ContentType).Returns("Foo");
        
        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object);
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ProcessAndCreateRecipeImage_FileExists_ReturnsExistingRecipeImage()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(NormalFileSize);
        file.Setup(f => f.ContentType).Returns("image/jpeg");

        var imageData = new byte[] { 1, 2, 3 };
        
        _recipeImageRepositoryMock.Setup(r => r.GetExistingImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync(new RecipeImage{ImageData = imageData});
        
        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object);
        
        Assert.That(result!.ImageData, Is.EqualTo(imageData));
    }
    
    [Test]
    public async Task ProcessAndCreateRecipeImage_ValidFile_ReturnsRecipeImage()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(NormalFileSize);
        file.Setup(f => f.ContentType).Returns("image/jpeg");
        
        var imagedata = new byte[] { 1, 2, 3 };
        using var memoryStream = new MemoryStream();
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, _) =>
                stream.Write(imagedata, 0, imagedata.Length));
        
        _recipeImageRepositoryMock.Setup(r => r.GetExistingImageAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync((RecipeImage?)null);
        
        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RecipeImage>());
            Assert.That(result!.ImageData, Has.Length.EqualTo(imagedata.Length));
            Assert.That(result.MimeType, Is.EqualTo(file.Object.ContentType));
        });
    }


}