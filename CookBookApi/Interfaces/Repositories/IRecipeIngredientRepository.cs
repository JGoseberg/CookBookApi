namespace CookBookApi.Interfaces.Repositories
{
    public interface IRecipeIngredientRepository
    {
        Task<bool> AnyRecipesWithIngredientAsync(int ingredientId);

        Task<bool> AnyRecipesWithMeasurementUnitAsync(int measurementUnitId);
    }
}
