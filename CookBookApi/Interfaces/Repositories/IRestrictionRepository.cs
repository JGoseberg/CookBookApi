using CookBookApi.DTOs;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Models;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IRestrictionRepository
    {
        Task<RestrictionDto?> GetRestrictionByIdAsync(int id);
        Task<IEnumerable<RestrictionDto>> GetAllRestrictionsAsync();
        Task AddRestrictionAsync(Restriction restriction);
        Task<bool> AnyRestrictionWithSameNameAsync(string name);
        Task UpdateRestrictionAsync(Restriction restriction);
        Task DeleteRestrictionAsync(int id);
    }
}
