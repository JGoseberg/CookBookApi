using System.Net;
using Castle.Components.DictionaryAdapter.Xml;
using CookBookApi.Interfaces;
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
    public async Task ProcessAndCreateRecipeImage_FileIsNull_ThrowsArgumentNullException()
    {
        IFormFile file = null;

        Assert.ThrowsAsync<ArgumentException>(async () =>
                await _recipeImageService.ProcessAndCreateRecipeImageAsync(file),
            "Invalid File");
    }

    [Test]
    public async Task ProcessAndCreateRecipeImage_FileIsEmpty_ThrowsArgumentNullException()
    {
        IFormFile file = null;
        
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _recipeImageService.ProcessAndCreateRecipeImageAsync(file),
            "Invalid File");
    }
    
    [Test]
    public async Task ProcessAndCreateRecipeImage_FileIsToBig_ThrowsArgumentException()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(BigFileSize);

        var ax = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object));
        Assert.That(ax.Message, Is.EqualTo(
            "File is too large. Please use only Files smaller than 5 MB"));
    }
    
    [Test]
    public async Task ProcessAndCreateRecipeImage_InvalidMimeType_ThrowsArgumentException()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(NormalFileSize);
        file.Setup(f => f.ContentType).Returns("image/gif");
        
        var ax = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object));
        Assert.That(ax.Message, Is.EqualTo("Unsupported File type Allowed are only jpg, jpeg, png, webp"));
    }

    [Test]
    public async Task ProcessAndCreateRecipeImage_FileExists_ReturnsExistingRecipeImage()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(NormalFileSize);
        file.Setup(f => f.ContentType).Returns("image/jpeg");
        
        _recipeImageRepositoryMock.Setup(r => r.ImageExistsAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        
        var ax = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object));
        Assert.That(ax.Message, Is.EqualTo("Image already exists"));
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
            .Callback<Stream, CancellationToken>((stream, token) =>
                stream.Write(imagedata, 0, imagedata.Length));
        
        _recipeImageRepositoryMock.Setup(r => r.ImageExistsAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        
        var result = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file.Object);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<RecipeImage>());
        Assert.Multiple(() =>
        {
            Assert.That(result.ImageData, Has.Length.EqualTo(imagedata.Length));
            Assert.That(result.MimeType, Is.EqualTo(file.Object.ContentType));
        });
    }


}