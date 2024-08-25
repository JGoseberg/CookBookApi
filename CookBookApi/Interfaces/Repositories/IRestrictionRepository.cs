using CookBookApi.DTOs;
using CookBookApi.Models;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IRestrictionRepository
    {
        Task<RecipeDto> GetRestrictionByIdAsync(int id);
        Task<IEnumerable<RecipeDto>> GetAllRestrictionsAsync();
        Task AddRestrictionAsync(Restriction restriction);
        Task UpdateRestrictionAsync(Restriction restriction);
        Task DeleteRestrictionAsync(int id);
    }
}
