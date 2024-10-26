using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class RestrictionRepository : IRestrictionRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;

        public RestrictionRepository(CookBookContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public Task AddRestrictionAsync(Restriction restriction)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyRestrictionWithSameNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRestrictionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RestrictionDto>> GetAllRestrictionsAsync()
        {
            var restrictions = await _context.Restrictions.ToListAsync();
            
            return _mapper.Map<IEnumerable<RestrictionDto>>(restrictions);
        }

        public Task<RestrictionDto?> GetRestrictionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRestrictionAsync(Restriction restriction)
        {
            throw new NotImplementedException();
        }
    }
}
