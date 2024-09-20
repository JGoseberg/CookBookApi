using AutoMapper;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public IngredientsController
            (
            IIngredientRepository ingredientRepository,
            IRecipeRepository recipeRepository,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
            _recipeRepository = recipeRepository;
        }

        [HttpPost]
        public async Task<ActionResult> AddIngredientAsync(IngredientDto ingredientDto)
        {
            if (ingredientDto.Name.IsNullOrEmpty())
                return BadRequest($"Name: cannot be empty");

            var isExisting = await _ingredientRepository.AnyIngredientWithSameName(ingredientDto.Name);
            if (isExisting)
                return BadRequest("An Ingredient with this Name already exists");

            var newIngredient = new Ingredient { Name = ingredientDto.Name };

            await _ingredientRepository.AddIngredientAsync(newIngredient);

            return Created("", newIngredient);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIngredientAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound($"Ingredient with id {id} not found");

            var hasRelatedRecipes = await _recipeRepository.AnyRecipeWithIngredientAsync(id);
            if (hasRelatedRecipes)
                return BadRequest($"Ingredient with id {id} has existing relations to recipes, please remove them first");

            await _ingredientRepository.DeleteIngredientAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDto>> GetIngredientByIdAsync(int id)
        {
            var ingredient = _ingredientRepository.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound($"Ingredient with id {id} not found");

            return Ok(ingredient);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<IngredientDto>> UpdateIngredientAsync(int id, IngredientDto ingredientDto)
        {
            var existingIngredient = await _ingredientRepository.GetIngredientByIdAsync(id);
            if (existingIngredient == null)
                return NotFound($"Ingredient with id {id} not found");

            if (await _ingredientRepository.AnyIngredientWithSameName(ingredientDto.Name))
                return BadRequest("A cuisine with this name already exists.");

            var updatedIngredient = new Ingredient { Id = id, Name = ingredientDto.Name };

            await _ingredientRepository.UpdateIngredientAsync(updatedIngredient);

            var updatedIngredientDto = _mapper.Map<IngredientDto>(updatedIngredient);
            return Ok(updatedIngredientDto);
        }


    }
}
