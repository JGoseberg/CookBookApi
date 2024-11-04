namespace CookBookApi.Interfaces.Repositories
{
    public interface IRecipeIngredientRepository
    {
        Task<IEnumerable<int>?> GetRecipesWithIngredientsAsync(List<int> ingredientId);
        Task<bool> AnyRecipesWithIngredientAsync(int ingredientId);

        Task<bool> AnyRecipesWithMeasurementUnitAsync(int measurementUnitId);
    }
}
