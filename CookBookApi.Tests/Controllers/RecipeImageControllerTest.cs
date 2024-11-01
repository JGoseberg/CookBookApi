using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookBookApi.Tests.Controllers;

[TestFixture]
public class RecipeImageControllerTest
{
    private Mock<IRecipeRepository> _recipeRepository;
    private Mock<IRecipeImageRepository> _recipeImageRepository;
    private Mock<IRecipeImageService> _recipeImageService;
    
    private Mock<IMapper> _mapper;
    
    private RecipeImageController _controller;
    
    [SetUp]
    public void Setup()
    {
        _recipeRepository = new Mock<IRecipeRepository>();
        _recipeImageRepository = new Mock<IRecipeImageRepository>();
        _recipeImageService = new Mock<IRecipeImageService>();
        _mapper = new Mock<IMapper>();
        _controller = new RecipeImageController(_recipeRepository.Object, _recipeImageRepository.Object, _recipeImageService.Object, _mapper.Object);
    }

    [Test]
    public async Task AddRecipeImage_RecipeNotFound_ReturnsNotFound()
    {
        var recipeId = 1;
        IFormFile file = null;
        
        _recipeRepository.Setup(rr => rr.GetRecipeByIdAsync(recipeId)).ReturnsAsync((RecipeDto?)null);
        
        var result = await _controller.AddRecipeImage(recipeId, file);
        
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task AddRecipeImage_ImageExists_ReturnsNotFound()
    {
        var recipeId = 1;
        
        var recipe = new RecipeDto { Id = recipeId };
        
        var recipeImage = new RecipeImage {Id = 1, RecipeId = recipe.Id};
        
        IFormFile file = null;
        
        _recipeRepository.Setup(rr => rr.GetRecipeByIdAsync(recipe.Id)).ReturnsAsync(recipe);
        _recipeImageRepository.Setup(ri => ri.ImageExistsAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _recipeImageService.Setup(ri => ri.ProcessAndCreateRecipeImageAsync(file)).ReturnsAsync(recipeImage);
        
        var result = await _controller.AddRecipeImage(recipeId, file);
        
        Assert.That(result, Is.TypeOf<CreatedResult>());
    }
    
    [Test]
    public async Task AddRecipeImage_ValidRecipe_ReturnsOk()
    {
        var recipeId = 1;
        
        var recipe = new RecipeDto { Id = recipeId };
        
        var recipeImage = new RecipeImage {Id = 1, RecipeId = recipe.Id};
        
        IFormFile file = null;
        
        _recipeRepository.Setup(rr => rr.GetRecipeByIdAsync(recipe.Id)).ReturnsAsync(recipe);
        _recipeImageService.Setup(ri => ri.ProcessAndCreateRecipeImageAsync(file)).ReturnsAsync(recipeImage);
        
        var result = await _controller.AddRecipeImage(recipeId, file);
        
        Assert.That(result, Is.TypeOf<CreatedResult>());
    }

    [Test]
    public async Task GetRecipeImage_ImagesNotFound_ReturnsNotFound()
    {
        var recipeId = 1;

        var result = await _controller.GetRecipeImagesByRecipeIdAsync(recipeId);
        
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetRecipeImage_ImagesFound_ReturnsOK()
    {
        var recipeId = 1;

        var recipeImages = new RecipeImage[]
        {
            new RecipeImage { Id = 1, RecipeId = recipeId, ImageData = new byte[] { 1, 2, 3 }, MimeType = "image/png" },
            new RecipeImage { Id = 2, RecipeId = recipeId, ImageData = new byte[] { 1, 2, 3 }, MimeType = "image/jpg" },
        };
        
        _recipeImageRepository.Setup(r => r.GetRecipeImagesAsync(recipeId)).ReturnsAsync(recipeImages);
        
        var result = await _controller.GetRecipeImagesByRecipeIdAsync(recipeId);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }
}