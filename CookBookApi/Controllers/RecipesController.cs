using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;

    public RecipesController(IRecipeRepository recipeRepository, IMapper mapper)
    {
        _mapper = mapper;
        _recipeRepository = recipeRepository;
    }

    // GET: api/Recipes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipesAsync()
    {
        var recipes = await _recipeRepository.GetAllRecipesAsync();

        return Ok(recipes);
    }

    // GET: api/Recipes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetRecipeByIdAsync(int id)
    {
        var recipe = await _recipeRepository.GetRecipeByIdAsync(id);

        return Ok(recipe);
    }

    // POST: api/Recipes
    [HttpPost("{id}")]
    public async Task<ActionResult<RecipeDto>> PostRecipeAsync(int id, RecipeDto recipeDto)
    {
        var recipe = _mapper.Map<Recipe>(recipeDto);

        await _recipeRepository.UpdateRecipeAsync(recipe);

        return Ok(recipe);
    }

    // PUT: api/Recipes/5
    [HttpPut]
    public async Task<ActionResult> PutRecipeAsync(RecipeDto recipeDto)
    { 
        var recipe = _mapper.Map<Recipe>(recipeDto);

        await _recipeRepository.AddRecipeAsync(recipe);

        return Ok(recipe);
    }

    // DELETE: api/Recipes/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipeAsync(int id)
    {
        await _recipeRepository.DeleteRecipeAsync(id);

        return NoContent();
    }
}
