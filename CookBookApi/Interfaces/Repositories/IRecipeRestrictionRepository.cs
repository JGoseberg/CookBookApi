namespace CookBookApi.Interfaces.Repositories;

public interface IRecipeRestrictionRepository
{
    Task<bool> AnyRecipeWithRestrictionAsync(int restrictionId);
}