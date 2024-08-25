using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using AutoMapper;

namespace CookBookApi.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;

        public IngredientRepository (CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task AddIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredientAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IngredientDto> GetIngredientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
