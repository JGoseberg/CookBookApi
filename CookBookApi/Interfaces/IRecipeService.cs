using CookBookApi.DTOs.Recipes;

namespace CookBookApi.Interfaces;

public interface IRecipeService
{
    Task<RecipeDto> GetRandomRecipeAsync();
}