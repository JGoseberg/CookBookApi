using CookBookApi.Models;

namespace CookBookApi.Interfaces;

public interface IRecipeImageService
{
    Task<RecipeImage?> ProcessAndCreateRecipeImageAsync(IFormFile file);
}