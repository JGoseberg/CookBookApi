using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            var cuisineToAdd = await _context.Cuisines.AddAsync(_mapper.Map<Cuisine>(cuisine));
            _context.SaveChanges();
        }

        public async Task DeleteCuisineAsync(int id)
        {
            var cuisineToDelete = await _context.Cuisines.FindAsync(id);

            if (cuisineToDelete == null)
                return;

            _context.Cuisines.Remove(cuisineToDelete);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync()
        {
            var cuisines = await _context.Cuisines.ToListAsync();

            return _mapper.Map<IEnumerable<CuisineDto>>(cuisines);
        }

        public async Task<CuisineDto> GetCuisineByIdAsync(int id)
        {
            var cuisine = await _context.Cuisines.FirstOrDefaultAsync(x => x.Id == id);

            if (cuisine == null)
                return null;

            return _mapper.Map<CuisineDto>(cuisine);
        }

        public async Task UpdateCuisineAsync(Cuisine cuisine)
        {
            var newCuisine = _context.Cuisines.Update(cuisine);
            await _context.SaveChangesAsync();
        }
    }
}
