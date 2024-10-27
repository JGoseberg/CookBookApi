using CookBookApi.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories;

public class IngredientRestrictionRepository : IIngredientRestrictionRepository
{
    private readonly CookBookContext _context;

    public IngredientRestrictionRepository(CookBookContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyIngredientWithRestrictionAsync(int restrictionId)
    {
        return await _context.IngredientRestrictions.AnyAsync(ir  => ir.RestrictionId == restrictionId);
    }
}