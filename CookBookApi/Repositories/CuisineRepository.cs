using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class CuisineRepository : ICuisineRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;
        public CuisineRepository(CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddCuisineAsync(Cuisine cuisine)
        {
            // TODO should work without mapper
            var cuisineToAdd = await _context.Cuisines.AddAsync(_mapper.Map<Cuisine>(cuisine));
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> AnyCuisineWithSameNameAsync(string name)
        {
            return await _context.Cuisines.AnyAsync(c => c.Name == name);
        }

        public async Task DeleteCuisineAsync(int id)
        {
            var cuisineToDelete = await _context.Cuisines.FindAsync(id);

            if (cuisineToDelete == null)
                return;

            _context.Cuisines.Remove(cuisineToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync()
        {
            var cuisines = await _context.Cuisines.ToListAsync();

            return _mapper.Map<IEnumerable<CuisineDto>>(cuisines);
        }

        public async Task<CuisineDto?> GetCuisineByIdAsync(int id)
        {
            var cuisine = await _context.Cuisines.FirstOrDefaultAsync(x => x.Id == id);

            return cuisine == null ? null : _mapper.Map<CuisineDto>(cuisine);
        }

        public async Task UpdateCuisineAsync(Cuisine cuisine)
        {
            var cuisineToUpdate = await _context.Cuisines.FindAsync(cuisine.Id);

            if (cuisineToUpdate != null)
                cuisineToUpdate.Name = cuisine.Name;
            
            await _context.SaveChangesAsync();
        }
    }
}
