using AutoMapper;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;

        public RecipeIngredientRepository(CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<int>?> GetRecipesWithIngredientAsync(int ingredientId)
        {
            var recipes = await _context.RecipeIngredients.Where(ri => ri.IngredientId == ingredientId).ToListAsync();

            if (recipes.Count == 0)
                return null;
            
            var recipeIds = recipes.Select(ri => ri.RecipeId).Distinct();
            
            return recipeIds;
        }

        public async Task<IEnumerable<int>?> GetRecipesWithIngredientsAsync(List<int> ingredientIds)
        {
            List<RecipeIngredient> recipeIngredients = null;

            foreach (var ingredientId in ingredientIds)
            {
                recipeIngredients.AddRange(await _context.RecipeIngredients.Where(ri => ri.IngredientId == ingredientId).ToListAsync());
            }
            
            if (recipeIngredients == null)
                return null;
            
            var recipeIds = recipeIngredients.Select(ri => ri.RecipeId).Distinct();
            
            return recipeIds;
        }
        
        public async Task<bool> AnyRecipesWithIngredientAsync(int ingredientId)
        {
            return await _context.Recipes.AnyAsync(r => r.RecipeIngredients.Any(ri => ri.IngredientId == ingredientId));
        }
        
        public async Task<bool> AnyRecipesWithMeasurementUnitAsync(int measurementUnitIdId)
        {
            return await _context.RecipeIngredients.AnyAsync(x => x.MeasurementUnitId == measurementUnitIdId);
        }
    }
}
