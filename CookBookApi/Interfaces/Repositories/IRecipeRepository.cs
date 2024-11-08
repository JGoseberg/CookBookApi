using CookBookApi.DTOs.Recipes;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        Task<bool> AnyRecipesWithCuisineAsync(int cuisineId);
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto?> GetRecipeByIdAsync(int id);
        Task<IEnumerable<RecipeDto?>> GetRecipesWithSpecificCuisineAsync(int cuisineId);
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int id);
    }
}
