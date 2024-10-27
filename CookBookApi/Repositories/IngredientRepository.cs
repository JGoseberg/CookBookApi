using CookBookApi.Interfaces.Repositories;
using AutoMapper;
using CookBookApi.DTOs.Ingredient;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;

        public IngredientRepository(CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyIngredientWithSameName(string name) 
        {
            // TODO add recipeRestrictionRepository and ingredientRestrictionRepository to implement if restriction is attached
            
            return await _context.Ingredients.AnyAsync(ingredient => ingredient.Name == name);
        }

        public async Task DeleteIngredientAsync(int id)
        {
            var ingredientToDelete = await _context.Ingredients.FindAsync(id);
            
            if (ingredientToDelete == null)
                return;
            
            _context.Ingredients.Remove(ingredientToDelete);
        }

        public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
        {
            var ingredients = await _context.Ingredients.ToListAsync();

            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public async Task<IngredientDto?> GetIngredientByIdAsync(int id)
        {
            var ingredient = await _context.Ingredients.FirstOrDefaultAsync(ingredient => ingredient.Id == id);
            
            return _mapper.Map<IngredientDto>(ingredient);
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            var ingredientToUpdate = await _context.Ingredients.FindAsync(ingredient.Id);

            if (ingredientToUpdate != null)
                ingredientToUpdate.Name = ingredient.Name;
            
            await _context.SaveChangesAsync();
        }
    }
}
