using CookBookApi.DTOs.Recipes;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        //TODO Method name is wrong
        Task<bool> AnyRecipesWithCuisineAsync(int cuisineId);
        Task<bool> AnyRecipeWithIngredientAsync(int ingredientId);
        Task<bool> AnyRecipesWithRestrictionAsync(int restrictionId);
        Task<RecipeDto> GetRecipeByIdAsync(int id);
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int id);
    }
}
