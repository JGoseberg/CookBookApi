using CookBookApi.DTOs;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int id);
    }
}
