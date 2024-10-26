using AutoMapper;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    public class RecipeIngredientController(
        IRecipeIngredientRepository recipeIngredientRepository,
        IRecipeRepository recipeRepository,
        IIngredientRepository ingredientRepository)
        : ControllerBase
    {
        private readonly IRecipeIngredientRepository _recipeIngredientRepository = recipeIngredientRepository;
        private readonly IRecipeRepository _recipeRepository = recipeRepository;
        private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    }
}
