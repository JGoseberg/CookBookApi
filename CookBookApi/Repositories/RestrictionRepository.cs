using CookBookApi.DTOs;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class RestrictionRepository : IRestrictionRepository
    {
        private readonly CookBookContext _context;

        public RestrictionRepository(CookBookContext context) 
        {
            _context = context;
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

        public Task<IEnumerable<RestrictionDto>> GetAllRestrictionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RestrictionDto> GetRestrictionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRestrictionAsync(Restriction restriction)
        {
            throw new NotImplementedException();
        }
    }
}
