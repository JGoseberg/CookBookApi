using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly ICuisineRepository _cuisineRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeIngredientRepository _recipeIngredientRepository;
    private readonly IRestrictionRepository _restrictionRepository;
    private readonly IMapper _mapper;
    public RecipesController(
        ICuisineRepository cuisineRepository,
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository,
        IMapper mapper, 
        IRecipeIngredientRepository recipeIngredientRepository,
        IRestrictionRepository restrictionRepository)
    {
        _cuisineRepository = cuisineRepository;
        _ingredientRepository = ingredientRepository;
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _recipeIngredientRepository = recipeIngredientRepository;
        _restrictionRepository = restrictionRepository;
    }
    [HttpPost]
    [ActionName("AddRecipe")]
    public async Task<ActionResult> AddRecipeAsync(AddRecipeDto recipeDto)
    {
        if (recipeDto.Name.IsNullOrEmpty())
            return BadRequest("Name Cannot be empty.");

        if (recipeDto.Instruction.IsNullOrEmpty())
            return BadRequest("Name Cannot be empty.");

        var subRecipes = new List<Recipe>();

        if (recipeDto.Subrecipes.Count != 0)
        {
            foreach (var subRecipe in recipeDto.Subrecipes)
            {
                var recipe = await _recipeRepository.GetRecipeByIdAsync(subRecipe.Id);

                subRecipes.Add(_mapper.Map<Recipe>(recipe));
            }
        }

        var parentRecipes = new List<Recipe>();

        if (recipeDto.ParentRecipes.Count != 0)
        {
            foreach (var parentRecipe in recipeDto.ParentRecipes)
            {
                var recipe = await _recipeRepository.GetRecipeByIdAsync(parentRecipe.Id);

                parentRecipes.Add(_mapper.Map<Recipe>(recipe));
            }
        }

        var newRecipe = new Recipe
        {
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Instruction = recipeDto.Instruction,
            Creator = recipeDto.Creator,
            CreateTime = DateTime.Now,
            CuisineId = recipeDto.CuisineId,
            Subrecipes = subRecipes,
            ParentRecipes = parentRecipes,
        };

        await _recipeRepository.AddRecipeAsync(newRecipe);

        return Created("", newRecipe);
    }

    [HttpDelete("{id}")]
    [ActionName("DeleteRecipe")]
    public async Task<ActionResult> DeleteRecipeAsync(int id)
    {
        var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
        if (recipe == null)
            return NotFound($"Recipe with Id: {id} not Found");

        await _recipeRepository.DeleteRecipeAsync(id);
        return NoContent();
    }

    [HttpGet]
    [ActionName("GetAllRecipes")]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllRecipesAsync()
    {
        var recipes = await _recipeRepository.GetAllRecipesAsync();

        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRandomRecipes")]
    public async Task<ActionResult<RecipeDto>> GetRandomRecipeAsync()
    {
        return await _recipeRepository.GetRandomRecipeAsync();
    }

    [HttpGet("{id}")]
    [ActionName("GetRecipeById")]
    public async Task<ActionResult<RecipeDto>> GetRecipeByIdAsync(int id)
    {
        var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
        if (recipe == null)
            return NotFound($"recipe with Id: {id} not found");

        return Ok(recipe);
    }

    [HttpGet("{cuisineId}")]
    [ActionName("GetRecipesByCuisine")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithCuisinesAsync(int cuisineId)
    {
        if (cuisineId == null)
            return BadRequest("cuisine Id cannot be empty");

        var cuisine = await _cuisineRepository.GetCuisineByIdAsync(cuisineId);
        if (cuisine == null)
            return BadRequest("Cuisine does not exists!");

        var recipes = await _recipeRepository.GetRecipesWithSpecificCuisineAsync(cuisineId);

        if (!recipes.Any())
            return NotFound($"No recipes with cuisine {cuisine.Name} found");

        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRecipesByIngredients")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithIngredientsAsync([FromQuery] List<int> ingredientIds)
    {
        if (ingredientIds.Count < 1)
            return BadRequest("Ingredient Ids cannot be empty");

        foreach (var ingredientId in ingredientIds)
        {
            if (await _ingredientRepository.GetIngredientByIdAsync(ingredientId) == null)
                return BadRequest($"Ingredient with Id: {ingredientId} not found");
        }

        var recipes = new List<RecipeDto?>();

        var recipeIds = await _recipeIngredientRepository.GetRecipesWithIngredientsAsync(ingredientIds);

        foreach (var recipeId in recipeIds)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);
            if (recipe != null)
                recipes.Add(recipe);
        }
        
        if (recipes.Count == 0)
            return BadRequest($"No Recipe with this Ingredients found!");
        
        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRecipesByRestriction")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithRestrictionsAsync([FromQuery] List<int> restrictionIds)
    {
        if (restrictionIds.Count < 1)
            return BadRequest("Restriction Ids cannot be empty");

        foreach (var restrictionId in restrictionIds)
        {
            if (await _restrictionRepository.GetRestrictionByIdAsync(restrictionId) == null)
                return BadRequest($"Restriction with Id: {restrictionId} not found");
        }

        var recipes = await _recipeRepository.GetRecipesWithRestrictionsAsync(restrictionIds);
        
        if (!recipes.Any())
            return BadRequest($"No Recipe with this Restrictions found!");
        
        return Ok(recipes);
    }

    [HttpPut("{id}")]
    [ActionName("UpdateRecipe")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipeAsync(int id, RecipeDto recipeDto)
    {
        var existingRecipe = await _recipeRepository.GetRecipeByIdAsync(id);
        if (existingRecipe == null)
            return NotFound($"Recipe With Id {id} not found");

        var recipeToUpdate = new Recipe
        {
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Instruction = recipeDto.Instruction,
            Cuisine = _mapper.Map<Cuisine>(recipeDto.Cuisine),
            Subrecipes = _mapper.Map<List<Recipe>>(recipeDto.Subrecipes),
            ParentRecipes = _mapper.Map<List<Recipe>>(recipeDto.ParentRecipes),
        };

        await _recipeRepository.UpdateRecipeAsync(recipeToUpdate);

        return Ok(recipeDto);
    }

}