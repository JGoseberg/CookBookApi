using CookBookApi.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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
}