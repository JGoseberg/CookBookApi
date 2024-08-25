using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly CookBookContext _context;

    public RecipesController(CookBookContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
    {
        return await _context.Recipes
            .Include(r => r.Subrecipes)
            .Include(r => r.Ingredients)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipe(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Subrecipes)
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            return NotFound();
        }

        return recipe;
    }

    [HttpPost]
    public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
    {
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
    {
        if (id != recipe.Id)
        {
            return BadRequest();
        }

        _context.Entry(recipe).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipes([FromQuery] string query)
    {
        return await _context.Recipes
            .Include(r => r.Subrecipes)
            .Include(r => r.Ingredients)
            .Where(r => r.Name.Contains(query) || r.Subrecipes.Any(s => s.Name.Contains(query)))
            .ToListAsync();
    }

    [HttpGet("by-ingredient/{ingredientName}")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByIngredient(string ingredientName)
    {
        return await _context.Recipes
            .Include(r => r.Subrecipes)
            .Include(r => r.Ingredients)
            .Where(r => r.Ingredients.Any(i => i.Name == ingredientName))
            .ToListAsync();
    }
}
