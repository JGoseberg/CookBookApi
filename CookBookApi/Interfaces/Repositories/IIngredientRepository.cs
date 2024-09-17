using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IIngredientRepository
    {
        Task<bool> AnyIngredientsWithRestrictionAsync(int id);
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int id);
    }
}
