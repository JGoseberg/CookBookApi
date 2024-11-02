using CookBookApi.DTOs.Ingredient;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IIngredientRepository
    {
        Task AddIngredientAsync(Ingredient ingredient);
        Task<bool> AnyIngredientWithSameNameAsync(string name);
        Task<IngredientDto?> GetIngredientByIdAsync(int id);
        Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int id);
    }
}
