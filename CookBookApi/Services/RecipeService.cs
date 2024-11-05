using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;

namespace CookBookApi.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _repository;

    public RecipeService(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<RecipeDto> GetRandomRecipeAsync()
    {
        var recipes = await _repository.GetAllRecipesAsync();
        
        var recipesCount = recipes.Count();
        
        var rnd = new Random((int)DateTime.Now.Ticks).Next(0, recipesCount);

        return recipes.ElementAt(rnd);
    }
}