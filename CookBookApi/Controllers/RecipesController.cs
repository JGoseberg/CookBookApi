using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
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
    private readonly IMapper _mapper;
    public RecipesController(
        ICuisineRepository cuisineRepository,
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository,
        IMapper mapper, 
        IRecipeIngredientRepository recipeIngredientRepository)
    {
        _cuisineRepository = cuisineRepository;
        _ingredientRepository = ingredientRepository;
        _recipeRepository = recipeRepository;
        _mapper = mapper;
        _recipeIngredientRepository = recipeIngredientRepository;
    }
    [HttpPost]
    [ActionName("AddRecipe")]
    public async Task<ActionResult> AddRecipeAsync(AddRecipeDto recipeDto)
    {
        if (recipeDto.Name.IsNullOrEmpty())
            return BadRequest("Name Cannot be empty");

        if (recipeDto.Instruction.IsNullOrEmpty())
            return BadRequest("Name Cannot be empty");

        var isExisting = await _recipeRepository.AnyRecipesWithSameNameAsync(recipeDto.Name);
        if (isExisting)
            return BadRequest("A Recipe with this Name already Exists");

        var subRecipes = new List<Recipe>();

        if (recipeDto.Subrecipes.Count == 0)
        {
            foreach (var subRecipe in recipeDto.Subrecipes)
            {
                var recipe = await _recipeRepository.GetRecipeByIdAsync(subRecipe.Id);

                subRecipes.Add(_mapper.Map<Recipe>(recipe));
            }
        }

        var parentRecipes = new List<Recipe>();

        if (recipeDto.ParentRecipes.Count == 0)
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
        var recipes = await _recipeRepository.GetAllRecipesAsync();

        var numberOfRecipes = recipes.Count();

        var rnd = new Random((int)DateTime.Now.Ticks).Next(1, numberOfRecipes+1);

        var randomRecipe = recipes.Where(r => r.Id == rnd);

        return Ok(randomRecipe);
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
            return BadRequest("cuisine does not exists");

        var recipes = await _recipeRepository.GetRecipesWithSpecificCuisineAsync(cuisineId);

        if (!recipes.Any())
            return NotFound($"No recipes with cuisine: {cuisine.Name} found");

        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRecipesByIngredients")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithIngredientsAsync([FromQuery] List<int> ingredientIds)
    {
        if (ingredientIds.Count() < 1)
            return BadRequest("Ingredient Ids cannot be empty");

        foreach (var ingredientId in ingredientIds)
        {
            if (await _ingredientRepository.GetIngredientByIdAsync(ingredientId) == null)
                return BadRequest($"Ingredient with Id: {ingredientId} not found");
        }

        var recipes = new List<RecipeDto>();
        
        var recipeIds = new List<int>();
        
        if (ingredientIds.Count() == 1)
        {
            recipeIds.AddRange(await _recipeIngredientRepository.GetRecipesWithIngredientAsync(ingredientIds[0]));

            if (recipeIds.Count == 0)
                return BadRequest($"No Recipe with Ingredient with Id: {ingredientIds[0]} found");
        }
        else // TODO NullReferenceException
        {
            recipeIds.AddRange(await _recipeIngredientRepository.GetRecipesWithIngredientsAsync(ingredientIds));
            
            if (recipeIds.Count == 0)
                return BadRequest($"No Recipe with Ingredient with Id: {ingredientIds[0]} found");
        }
        
        foreach (var recipeId in recipeIds)
        {
            recipes.Add(await _recipeRepository.GetRecipeByIdAsync(recipeId));   
        }
        
        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRecipesByRestriction")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithRestrictionsAsync(List<RestrictionDto> restrictions)
    {
        if (restrictions.Count() == 0)
            return BadRequest("select one or more ingredients");

        foreach (var restriction in restrictions)
            if (_ingredientRepository.GetIngredientByIdAsync(restriction.Id) == null)
                return BadRequest($"ingredient with name: {restriction.Name} does not exist");

        var recipes = await _recipeRepository.GetRecipesWithSpecificRestrictionsAsync(restrictions);

        if (recipes.Count() == 0)
            return NotFound($"No recipes with specified ingredients found");

        return Ok(recipes);
    }

    [HttpPut("{id}")]
    [ActionName("UpdateRecipe")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipeAsync(int id, RecipeDto recipeDto)
    {
        var existingRecipe = await _recipeRepository.GetRecipeByIdAsync(id);
        if (existingRecipe == null)
            return NotFound($"Recipe With Id {id} not found");

        if (await _recipeRepository.AnyRecipesWithSameNameAsync(recipeDto.Name))
            return BadRequest("A Recipe With this Name Already exists");

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