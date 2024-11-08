using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;
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
        if (recipeImage == null)
            return BadRequest("Recipe image could not be created. Please make sure it meets all requirements.");
        
        recipeImage.RecipeId = recipeId; 
        await _recipeImageRepository.AddRecipeImageAsync(recipeImage);
        
        var recipeImageDto = _mapper.Map<RecipeImageDto>(recipeImage);

        return Created("", recipeImageDto);
    }
    
    [HttpGet]
    [ActionName("GetRecipeImages")]
    public async Task<ActionResult> GetRecipeImagesByRecipeIdAsync(int recipeId)
    {
        var recipeImages = await _recipeImageRepository.GetRecipeImagesAsync(recipeId);

        var recipeImagesList = recipeImages.ToList();
        if(!recipeImagesList.Any())
            return NotFound("No images found");

        var images = new List<FileContentResult>();

        foreach (var recipeImage in recipeImagesList)
        {
            images.Add(File(recipeImage.ImageData, recipeImage.MimeType));
        }
        
        return Ok(images[0]);
    }
}