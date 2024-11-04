using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces.Repositories;
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

        public async Task<bool> AnyRecipesWithCuisineAsync(int cuisineId)
        {
            return await _context.Recipes.AnyAsync(r => r.CuisineId == cuisineId);
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

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
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

        public async Task<RecipeDto> GetRandomRecipeAsync()
        {
            //put in Service
            
            var recipes = await GetAllRecipesAsync();

            var numberOfRecipes = recipes.Count();

            var rnd = new Random((int)DateTime.Now.Ticks).Next(1, numberOfRecipes+1);

            var randomRecipe = recipes.Where(r => r.Id == rnd);

            return _mapper.Map<RecipeDto>(randomRecipe);
        }

        public async Task<IEnumerable<RecipeDto?>> GetRecipesWithSpecificCuisineAsync(int cuisineId)
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
                .Include(r => r.Subrecipes)
                .Where(r => r.CuisineId == cuisineId).ToListAsync();

            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public Task<IEnumerable<RecipeDto?>> GetRecipesWithRestrictionsAsync(IEnumerable<int> restrictionIds)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
