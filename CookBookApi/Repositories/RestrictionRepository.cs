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
        public async Task AddRestrictionAsync(Restriction restriction)
        {
            await _context.Restrictions.AddAsync(restriction);
            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyRestrictionWithSameNameAsync(string name)
        {
            return await _context.Restrictions.AnyAsync(x => x.Name == name);
        }

        public async Task DeleteRestrictionAsync(int id)
        {
            var restriction = await _context.Restrictions.FindAsync(id);
            
            if (restriction == null)
                return;
            
            _context.Restrictions.Remove(restriction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RestrictionDto>> GetAllRestrictionsAsync()
        {
            var restrictions = await _context.Restrictions.ToListAsync();
            
            return _mapper.Map<IEnumerable<RestrictionDto>>(restrictions);
        }

        public async Task<RestrictionDto?> GetRestrictionByIdAsync(int id)
        {
            var restriction = await _context.Restrictions.FirstOrDefaultAsync(x => x.Id == id);
            
            return _mapper.Map<RestrictionDto>(restriction);
        }

        public async Task UpdateRestrictionAsync(Restriction restriction)
        {
            var existingRestriction = await _context.Restrictions.FirstOrDefaultAsync(x => x.Id == restriction.Id);
            
            if (existingRestriction != null)
                existingRestriction.Name = restriction.Name;
            
            await _context.SaveChangesAsync();
        }
    }
}
