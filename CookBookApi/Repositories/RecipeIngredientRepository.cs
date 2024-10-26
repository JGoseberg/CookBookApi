using AutoMapper;
using CookBookApi.Interfaces.Repositories;
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
