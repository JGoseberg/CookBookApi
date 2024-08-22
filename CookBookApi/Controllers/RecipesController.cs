using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookBookApi.Data;
using CookBookApi.Models;
using CookBookApi.DTOs;

namespace CookBookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly CookBookContext _context;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(CookBookContext context, ILogger<RecipesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get: api/Recipe/int
        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetAll()
        {
            return _context.Recipes.ToList();
        }

        // GET: api/Recipe/int
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDetailDto>> GetRecipe(int id)
        {   
            if(int.IsNegative(id))
                return BadRequest();

            var recipe = await _context.Recipes
                .Include(r=> r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Measurement)
                        .ThenInclude(m => m.MeasurementUnit)
                .Include(r => r.RecipeDietaryRestrictions)
                    .ThenInclude(rd => rd.DietaryRestriction)
                .Include(r => r.ChildRecipes)
                    .ThenInclude(rr => rr.ChildRecipe)
                .Include(r => r.ParentRecipes)
                    .ThenInclude(rr => rr.ParentRecipe)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
            
            if(recipe == null)
                return NotFound();

            var recipeDetailDto = new RecipeDetailDto
            {
                RecipeId = recipe.RecipeId,
                RecipeName = recipe.RecipeName,
                RecipeDescription = recipe.Instruction,
                Rating = recipe.Rating,
                CountryKitchen = recipe.CountryKitchen,
                Ingredients = recipe.RecipeIngredients.Select(ri => new IngredientDetailDto
                {
                    IngredientName = ri.Ingredient.IngredientName,
                    IngredientAmount = ri.Measurement.Amount,
                    MeasurementUnit = ri.Measurement.MeasurementUnit.MeasurementUnitName,
                }).ToList(),
                DietaryRestrictions = recipe.RecipeDietaryRestrictions.Select(rd => rd.DietaryRestriction.DietaryRestrictionName).ToList(),
                RelatedRecips = recipe.ChildRecipes.Select(cr => cr.ChildRecipe.RecipeName).ToList()                
            };

            return recipeDetailDto;
        }
    }
}
