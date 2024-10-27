using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.DTOs.Recipes;


namespace CookBookApi.Interfaces.Repositories
{
    public interface IRecipeRepository
    {
        //TODO Method name is wrong
        Task<bool> AnyRecipesWithCuisineAsync(int cuisineId);
        Task<bool> AnyRecipesWithSameNameAsync(string name);
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto?> GetRecipeByIdAsync(int id);
        Task<IEnumerable<RecipeDto?>> GetRecipesWithSpecificCuisineAsync(int cuisineId);
        Task<IEnumerable<RecipeDto>> GetRecipesWithSpecificIngredientAsync(int ingredientId);
        Task<IEnumerable<RecipeDto?>> GetRecipesWithSpecificIngredientsAsync(IEnumerable<int> ingredientIds);
        Task<IEnumerable<RecipeDto?>> GetRecipesWithSpecificRestrictionsAsync(IEnumerable<RestrictionDto> restrictionDto);
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int id);
    }
}
