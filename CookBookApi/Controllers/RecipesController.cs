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
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
    {
        //var recipes = await _context.Recipes
        //.Include(r => r.Ingredients)
        //.ThenInclude(i => i.MeasurementUnit)
        //.Include(r => r.Cuisine)
        //.Include(r => r.RecipeRestrictions)
        //.ThenInclude(rr => rr.Restriction)
        //.Include(r => r.Subrecipes)
        //.ThenInclude(sr => sr.Cuisine)
        //.ToListAsync();

        //var recipeDtos = recipes.Select(r => _mapper.Map<RecipeDto>(r)).ToList();
        //return Ok(recipeDtos);

        var recipes = await _recipeRepository.GetAllRecipesAsync();

        return Ok(recipes);
    }

    // GET: api/Recipes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
    {
        //var recipe = await _context.Recipes
        //    .Include(r => r.Ingredients)
        //        .ThenInclude(i => i.MeasurementUnit)
        //    .Include(r => r.Cuisine)
        //    .Include(r => r.RecipeRestrictions)
        //        .ThenInclude(rr => rr.Restriction)
        //        .ThenInclude(rr => rr.IngredientRestrictions)
        //    .Include(r => r.Subrecipes)
        //        .ThenInclude(sr => sr.Cuisine)
        //    .FirstOrDefaultAsync(r => r.Id == id);


        //if (recipe == null)
        //{
        //    return NotFound();
        //}

        //var recipeDto = _mapper.Map<RecipeDto>(recipe);
        //return Ok(recipeDto);

        var recipe = await _recipeRepository.GetRecipeByIdAsync(id);

        return Ok(recipe);
    }

    // POST: api/Recipes
    [HttpPost]
    public async Task<ActionResult<RecipeDto>> PostRecipe(RecipeDto recipeDto)
    {
        //var recipe = _mapper.Map<Recipe>(recipeDto);

        //_context.Recipes.Add(recipe);
        //await _context.SaveChangesAsync();

        //recipeDto.Id = recipe.Id; // Set the Id to the newly created recipe Id
        //return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipeDto);

        var recipe = _mapper.Map<Recipe>(recipeDto);

        await _recipeRepository.UpdateRecipeAsync(recipe);

        return Ok(recipe);
    }

    // PUT: api/Recipes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRecipe(int id, RecipeDto recipeDto)
    {
        //if (id != recipeDto.Id)
        //{
        //    return BadRequest();
        //}

        //var recipe = _mapper.Map<Recipe>(recipeDto);

        //_context.Entry(recipe).State = EntityState.Modified;

        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!RecipeExists(id))
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        throw;
        //    }
        //}

        //return NoContent();

        var recipe = _mapper.Map<Recipe>(recipeDto);

        await _recipeRepository.AddRecipeAsync(recipe);

        return Ok(recipe);
    }

    // DELETE: api/Recipes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
    //    var recipe = await _context.Recipes.FindAsync(id);
    //    if (recipe == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.Recipes.Remove(recipe);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool RecipeExists(int id)
    //{
    //    return _context.Recipes.Any(e => e.Id == id);

        await _recipeRepository.DeleteRecipeAsync(id);

        return NoContent();
    }
}
