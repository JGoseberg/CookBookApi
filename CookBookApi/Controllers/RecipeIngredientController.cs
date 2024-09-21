using AutoMapper;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    public class RecipeIngredientController : ControllerBase
    {
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;

        public RecipeIngredientController
            (
            IRecipeIngredientRepository recipeIngredientRepository, 
            IRecipeRepository recipeRepository, 
            IIngredientRepository ingredientRepository
            )
        {
            _recipeIngredientRepository = recipeIngredientRepository;
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
        }
    }
}
