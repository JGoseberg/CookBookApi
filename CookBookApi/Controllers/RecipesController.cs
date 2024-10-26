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
public class RecipesController(
    ICuisineRepository cuisineRepository,
    IIngredientRepository ingredientRepository,
    IRecipeRepository recipeRepository,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [ActionName("AddRecipe")]
    public async Task<ActionResult> AddRecipeAsync(AddRecipeDto recipeDto)
    {
        if (recipeDto.Name.IsNullOrEmpty())
            return BadRequest("Name Cannot be empty");

        if (recipeDto.Instruction.IsNullOrEmpty())
            return BadRequest("Name Cannot be empty");

        var isExisting = await recipeRepository.AnyRecipesWithSameNameAsync(recipeDto.Name);
        if (isExisting)
            return BadRequest("A Recipe with this Name already Exists");

        var subRecipes = new List<Recipe>();

        if (recipeDto.Subrecipes.Count == 0)
        {
            foreach (var subRecipe in recipeDto.Subrecipes)
            {
                var recipe = await recipeRepository.GetRecipeByIdAsync(subRecipe.Id);

                subRecipes.Add(mapper.Map<Recipe>(recipe));
            }
        }

        var parentRecipes = new List<Recipe>();

        if (recipeDto.ParentRecipes.Count == 0)
        {
            foreach (var parentRecipe in recipeDto.ParentRecipes)
            {
                var recipe = await recipeRepository.GetRecipeByIdAsync(parentRecipe.Id);

                parentRecipes.Add(mapper.Map<Recipe>(recipe));
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

        await recipeRepository.AddRecipeAsync(newRecipe);

        return Created("", newRecipe);
    }

    [HttpDelete("{id}")]
    [ActionName("DeleteRecipe")]
    public async Task<ActionResult> DeleteRecipeAsync(int id)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(id);
        if (recipe == null)
            return NotFound($"Recipe with Id: {id} not Found");

        await recipeRepository.DeleteRecipeAsync(id);
        return NoContent();
    }

    [HttpGet]
    [ActionName("GetRecipes")]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllRecipesAsync()
    {
        var recipes = await recipeRepository.GetAllRecipesAsync();

        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRandomRecipes")]
    public async Task<ActionResult<RecipeDto>> GetRandomRecipeAsync()
    {
        var recipes = await recipeRepository.GetAllRecipesAsync();

        var numberOfRecipes = recipes.Count();

        var rnd = new Random((int)DateTime.Now.Ticks).Next(0, numberOfRecipes);

        var randomRecipe = recipes.Where(r => r.Id == rnd);

        return Ok(randomRecipe);
    }

    [HttpGet("{id}")]
    [ActionName("GetRecipeById")]
    public async Task<ActionResult<RecipeDto>> GetRecipeByIdAsync(int id)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(id);
        if (recipe == null)
            return NotFound($"recipe with Id: {id} not found");

        return Ok(recipe);
    }

    [HttpGet]
    [ActionName("GetRecipesByCuisine")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithCuisinesAsync(CuisineDto cuisineDto)
    {
        if (cuisineDto == null)
            return BadRequest("cuisine cannot be empty");

        if (await cuisineRepository.GetCuisineByIdAsync(cuisineDto.Id) == null)
            return BadRequest("cuisine does not exists");

        var recipes = await recipeRepository.GetRecipesWithSpecificCuisineAsync(cuisineDto);

        if (!recipes.Any())
            return NotFound($"No recipes with cuisine: {cuisineDto.Name} found");

        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRecipesByIngredient")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithIngredientsAsync(List<IngredientDto> ingredients)
    {
        if (ingredients.Count == 0)
            return BadRequest("select one or more ingredients");

        foreach (var ingredient in ingredients)
            if (ingredientRepository.GetIngredientByIdAsync(ingredient.Id) == null)
                return BadRequest($"ingredient with name: {ingredient.Name} does not exist");

        var recipes = await recipeRepository.GetRecipesWithSpecificIngredientsAsync(ingredients);

        if (recipes.Count() == 0)
            return NotFound($"No recipes with specified ingredients found");

        return Ok(recipes);
    }

    [HttpGet]
    [ActionName("GetRecipesByRestriction")]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithRestrictionsAsync(List<RestrictionDto> restrictions)
    {
        if (restrictions.Count() == 0)
            return BadRequest("select one or more ingredients");

        foreach (var restriction in restrictions)
            if (ingredientRepository.GetIngredientByIdAsync(restriction.Id) == null)
                return BadRequest($"ingredient with name: {restriction.Name} does not exist");

        var recipes = await recipeRepository.GetRecipesWithSpecificRestrictionsAsync(restrictions);

        if (recipes.Count() == 0)
            return NotFound($"No recipes with specified ingredients found");

        return Ok(recipes);
    }

    [HttpPut("{id}")]
    [ActionName("UpdateRecipe")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipeAsync(int id, RecipeDto recipeDto)
    {
        var existingRecipe = await recipeRepository.GetRecipeByIdAsync(id);
        if (existingRecipe == null)
            return NotFound($"Recipe With Id {id} not found");

        if (await recipeRepository.AnyRecipesWithSameNameAsync(recipeDto.Name))
            return BadRequest("A Recipe With this Name Already exists");

        var recipeToUpdate = new Recipe
        {
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Instruction = recipeDto.Instruction,
            Cuisine = mapper.Map<Cuisine>(recipeDto.Cuisine),
            Subrecipes = mapper.Map<List<Recipe>>(recipeDto.Subrecipes),
            ParentRecipes = mapper.Map<List<Recipe>>(recipeDto.ParentRecipes),
        };

        await recipeRepository.UpdateRecipeAsync(recipeToUpdate);

        return Ok(recipeDto);
    }

}