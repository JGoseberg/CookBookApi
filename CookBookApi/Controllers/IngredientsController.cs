using AutoMapper;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly ICuisineRepository _cuisineRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public IngredientsController(
            ICuisineRepository cuisineRepository,
            IIngredientRepository ingredientRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            IMapper mapper)
        {
            _cuisineRepository = cuisineRepository;
            _ingredientRepository = ingredientRepository;
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }
        [HttpPost]
        [ActionName("AddIngredient")]
        public async Task<ActionResult> AddIngredientAsync(string name, int cuisineId)
        {
            if (name.IsNullOrEmpty())
                return BadRequest($"Name cannot be empty");

            if (await _cuisineRepository.GetCuisineByIdAsync(cuisineId) == null)
                return BadRequest($"CuisineId cannot be null");
            
            var isExisting = await _ingredientRepository.AnyIngredientWithSameNameAsync(name);
            if (isExisting)
                return BadRequest("An Ingredient with this Name already exists");
            
            var newIngredient = new Ingredient { Name = name, CuisineId = cuisineId };

            await _ingredientRepository.AddIngredientAsync(newIngredient);

            return Created("", newIngredient);
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteIngredient")]
        public async Task<ActionResult> DeleteIngredientAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound($"Ingredient with id {id} not found");

            var hasRelatedRecipes = await _recipeIngredientRepository.AnyRecipesWithIngredientAsync(id);
            if (hasRelatedRecipes)
                return BadRequest($"Ingredient with id {id} has existing relations to recipes, please remove them first");

            await _ingredientRepository.DeleteIngredientAsync(id);
            return NoContent();
        }

        [HttpGet]
        [ActionName("GetAllIngredients")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAllIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        [ActionName("GetIngredientById")]
        public async Task<ActionResult<IngredientDto>> GetIngredientByIdAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound($"Ingredient with id {id} not found");

            return Ok(ingredient);
        }


        [HttpPut("{id}")]
        [ActionName("UpdateIngredient")]
        public async Task<ActionResult<IngredientDto>> UpdateIngredientAsync(int id, IngredientDto ingredientDto)
        {
            var existingIngredient = await _ingredientRepository.GetIngredientByIdAsync(id);
            if (existingIngredient == null)
                return NotFound($"Ingredient with id {id} not found");

            if (await _ingredientRepository.AnyIngredientWithSameNameAsync(ingredientDto.Name))
                return BadRequest("A Ingredient with this name already exists.");

            var updatedIngredient = new Ingredient { Id = id, Name = ingredientDto.Name };

            await _ingredientRepository.UpdateIngredientAsync(updatedIngredient);

            var updatedIngredientDto = _mapper.Map<IngredientDto>(updatedIngredient);
            return Ok(updatedIngredientDto);
        }
    }
}
