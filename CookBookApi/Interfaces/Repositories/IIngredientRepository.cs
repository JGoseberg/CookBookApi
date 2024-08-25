using CookBookApi.DTOs;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IIngredientRepository
    {
        Task<IngredientDto> GetIngredientByIdAsync(int id);
        Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int id);
    }
}
