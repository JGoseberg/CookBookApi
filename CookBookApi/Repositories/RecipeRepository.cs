using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;

        public RecipeRepository(CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            _context.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Cuisine)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.MeasurementUnit)
                .Include(r => r.RecipeRestrictions)
                    .ThenInclude(rr => rr.Restriction)
                    .ThenInclude(rr => rr.IngredientRestrictions)
                .Include(r => r.Subrecipes).ToListAsync();

            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public async Task<RecipeDto> GetRecipeByIdAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Cuisine)

                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)

                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.MeasurementUnit)

                .Include(r => r.RecipeRestrictions)
                    .ThenInclude(rr => rr.Restriction)
                    .ThenInclude(rr => rr.IngredientRestrictions)
                .Include(r => r.Subrecipes)
                .FirstOrDefaultAsync(r => r.Id == id);

            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
