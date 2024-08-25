using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;

namespace CookBookApi.Repositories
{
    public class RestrictionRepository : IRestrictionRepository
    {
        public Task AddRestrictionAsync(Restriction restriction)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRestrictionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecipeDto>> GetAllRestrictionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDto> GetRestrictionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRestrictionAsync(Restriction restriction)
        {
            throw new NotImplementedException();
        }
    }
}
