namespace CookBookApi.Interfaces.Repositories
{
    public interface IRecipeIngredientRepository
    {
        Task<bool> AnyRecipesWithMeasurementUnitAsync(int measurementUnitId);
    }
}
