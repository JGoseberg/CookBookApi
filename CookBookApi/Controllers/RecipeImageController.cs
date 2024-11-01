using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RecipeImageController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeImageRepository _recipeImageRepository;
    private readonly IRecipeImageService _recipeImageService;
    private readonly IMapper _mapper;

    public RecipeImageController(
        IRecipeRepository recipeRepository, 
        IRecipeImageRepository recipeImageRepository,
        IRecipeImageService recipeImageService,
        IMapper mapper)
    {
        _recipeImageRepository = recipeImageRepository;
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _recipeImageService = recipeImageService;
    }

    [HttpPost]
    [ActionName("AddRecipeImage")]
    public async Task<ActionResult> AddRecipeImage(int recipeId, IFormFile file)
    {
        var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);

        if (recipe == null)
            return NotFound($"Recipe with id {recipeId} not found");
        
        var recipeImage = await _recipeImageService.ProcessAndCreateRecipeImageAsync(file);
        recipeImage.RecipeId = recipeId;
        
        var recipeImageDto = _mapper.Map<RecipeImageDto>(recipeImage);
        
        await _recipeImageRepository.AddRecipeImageAsync(recipeImage);
        
        return Created("", recipeImageDto);
    }
    
    [HttpGet]
    [ActionName("GetRecipeImages")]
    public async Task<ActionResult> GetRecipeImagesByRecipeIdAsync(int recipeId)
    {
        var recipeImages = await _recipeImageRepository.GetRecipeImagesAsync(recipeId);

        if(!recipeImages.Any())
            return NotFound("No images found");

        var images = new List<FileContentResult>();

        foreach (var recipeImage in recipeImages)
        {
            images.Add(File(recipeImage.ImageData, recipeImage.MimeType));
        }
        
        return Ok(images[0]);
    }
}