using AutoMapper;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController(
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository,
        IMapper mapper)
        : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddIngredientAsync(IngredientDto ingredientDto)
        {
            if (ingredientDto.Name.IsNullOrEmpty())
                return BadRequest($"Name: cannot be empty");

            var isExisting = await ingredientRepository.AnyIngredientWithSameName(ingredientDto.Name);
            if (isExisting)
                return BadRequest("An Ingredient with this Name already exists");

            var newIngredient = new Ingredient { Name = ingredientDto.Name };

            await ingredientRepository.AddIngredientAsync(newIngredient);

            return Created("", newIngredient);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIngredientAsync(int id)
        {
            var ingredient = await ingredientRepository.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound($"Ingredient with id {id} not found");

            var hasRelatedRecipes = await recipeRepository.AnyRecipeWithIngredientAsync(id);
            if (hasRelatedRecipes)
                return BadRequest($"Ingredient with id {id} has existing relations to recipes, please remove them first");

            await ingredientRepository.DeleteIngredientAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredientsAsync()
        {
            var ingredients = await ingredientRepository.GetAllIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDto>> GetIngredientByIdAsync(int id)
        {
            var ingredient = await ingredientRepository.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound($"Ingredient with id {id} not found");

            return Ok(ingredient);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<IngredientDto>> UpdateIngredientAsync(int id, IngredientDto ingredientDto)
        {
            var existingIngredient = await ingredientRepository.GetIngredientByIdAsync(id);
            if (existingIngredient == null)
                return NotFound($"Ingredient with id {id} not found");

            if (await ingredientRepository.AnyIngredientWithSameName(ingredientDto.Name))
                return BadRequest("A Ingredient with this name already exists.");

            var updatedIngredient = new Ingredient { Id = id, Name = ingredientDto.Name };

            await ingredientRepository.UpdateIngredientAsync(updatedIngredient);

            var updatedIngredientDto = mapper.Map<IngredientDto>(updatedIngredient);
            return Ok(updatedIngredientDto);
        }


    }
}
