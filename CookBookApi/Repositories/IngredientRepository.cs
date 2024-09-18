using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using CookBookApi.DTOs.Ingredient;

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

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
            _context.SaveChanges();
        }

        public Task<bool> AnyIngredientsWithRestrictionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredientAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            var ingredients = await _context.Ingredients
                .ToListAsync();

            return (IEnumerable<Ingredient>)ingredients;
        }

        public Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredientAsync(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
