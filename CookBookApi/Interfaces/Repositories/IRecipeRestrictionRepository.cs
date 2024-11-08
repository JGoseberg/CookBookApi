namespace CookBookApi.Interfaces.Repositories;

public interface IRecipeRestrictionRepository
{
    Task<bool> AnyRecipeWithRestrictionAsync(int restrictionId);
    Task<IEnumerable<int>>GetRecipeIdsWithRestrictionAsync(List<int> restrictionId);
}