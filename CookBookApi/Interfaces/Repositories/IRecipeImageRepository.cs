using CookBookApi.Models;

namespace CookBookApi.Interfaces.Repositories;

public interface IRecipeImageRepository
{
    public Task AddRecipeImageAsync(RecipeImage recipeImage);
    public Task<IEnumerable<RecipeImage>> GetRecipeImagesAsync(int recipeId);
    public Task<bool> ImageExistsAsync(byte[] imageData, string mimeType);
}