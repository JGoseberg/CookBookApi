using CookBookApi.Interfaces.Repositories;
using AutoMapper;
using CookBookApi.DTOs.Ingredient;

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
            _context.SaveChanges();
        }

        public Task<bool> AnyIngredientWithRestrictionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyIngredientWithSameName(string name) 
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredientAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IngredientDto?> GetIngredientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
