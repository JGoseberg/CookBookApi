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
        public Task AddCuisineAsync(Cuisine cuisine)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCuisineAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync()
        {
            var cuisines = await _context.Cuisines.ToListAsync();

            return _mapper.Map<IEnumerable<CuisineDto>>(cuisines);
        }

        public Task<CuisineDto> GetCuisineByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCuisineAsync(Cuisine cuisine)
        {
            throw new NotImplementedException();
        }
    }
}
