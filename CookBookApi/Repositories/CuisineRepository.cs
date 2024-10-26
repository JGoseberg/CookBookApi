using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class CuisineRepository(CookBookContext context, IMapper mapper) : ICuisineRepository
    {
        public async Task AddCuisineAsync(Cuisine cuisine)
        {
            // TODO should work without mapper
            var cuisineToAdd = await context.Cuisines.AddAsync(mapper.Map<Cuisine>(cuisine));
            await context.SaveChangesAsync();
        }
        
        public async Task<bool> AnyCuisineWithSameNameAsync(string name)
        {
            return await context.Cuisines.AnyAsync(c => c.Name == name);
        }

        public async Task DeleteCuisineAsync(int id)
        {
            var cuisineToDelete = await context.Cuisines.FindAsync(id);

            if (cuisineToDelete == null)
                return;

            context.Cuisines.Remove(cuisineToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync()
        {
            var cuisines = await context.Cuisines.ToListAsync();

            return mapper.Map<IEnumerable<CuisineDto>>(cuisines);
        }

        public async Task<CuisineDto?> GetCuisineByIdAsync(int id)
        {
            var cuisine = await context.Cuisines.FirstOrDefaultAsync(x => x.Id == id);

            return cuisine == null ? null : mapper.Map<CuisineDto>(cuisine);
        }

        public async Task UpdateCuisineAsync(Cuisine cuisine)
        {
            var cuisineToUpdate = await context.Cuisines.FindAsync(cuisine.Id);

            if (cuisineToUpdate != null)
                cuisineToUpdate.Name = cuisine.Name;
            
            await context.SaveChangesAsync();
        }

    }
}
