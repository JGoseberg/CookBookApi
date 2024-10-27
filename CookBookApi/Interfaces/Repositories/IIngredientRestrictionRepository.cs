namespace CookBookApi.Interfaces.Repositories;

public interface IIngredientRestrictionRepository
{
    Task<bool> AnyIngredientWithRestrictionAsync(int restrictionId);
}