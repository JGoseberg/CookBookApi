using CookBookApi.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Repositories;

public class RecipeRestrictionRepository : IRecipeRestrictionRepository
{
    private readonly CookBookContext _context;

    public RecipeRestrictionRepository(CookBookContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyRecipeWithRestrictionAsync(int restrictionId)
    {
        return await _context.RecipeRestrictions.AnyAsync(rr => rr.RestrictionId == restrictionId);
    }

    public async Task<IEnumerable<int>?> GetRecipeIdsWithRestrictionAsync(List<int> restrictionIds)
    {
        if (restrictionIds.IsNullOrEmpty())
            return null;
        
        var recipeIds = await _context.RecipeRestrictions
            .Where(rr => restrictionIds.Contains(rr.RestrictionId))
            .GroupBy(rr => rr.RecipeId)
            .Where(group => group
                .Select(rr => rr.RestrictionId).Distinct()
                .Count() == restrictionIds.Count)
            .Select(group => group.Key)
            .ToListAsync();
        
        return recipeIds;
    }
}