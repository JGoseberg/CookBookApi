using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RestrictionsController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IIngredientRestrictionRepository _ingredientRestrictionRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IRecipeRestrictionRepository _recipeRestrictionRepository;
        private readonly IRestrictionRepository _restrictionRepository;

        public RestrictionsController(
            IIngredientRepository ingredientRepository,
            IIngredientRestrictionRepository ingredientRestrictionRepository,
            IRecipeRepository recipeRepository,
            IRecipeRestrictionRepository recipeRestrictionRepository,
            IRestrictionRepository restrictionRepository
            )
        {
            _ingredientRepository = ingredientRepository;
            _ingredientRestrictionRepository = ingredientRestrictionRepository;
            _recipeRepository = recipeRepository;
            _recipeRestrictionRepository = recipeRestrictionRepository;
            _restrictionRepository = restrictionRepository;
        }

        [HttpPost]
        [ActionName("AddRestriction")]
        public async Task<ActionResult> AddRestrictionAsync(RestrictionDto restrictionDto)
        {
            if (restrictionDto.Name.IsNullOrEmpty())
                return BadRequest($"Name {restrictionDto.Name} cannot be empty");

            if (await _restrictionRepository.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                return BadRequest($"A Restriction with the Name {restrictionDto.Name} already exists");

            var newRestriction = new Restriction { Name = restrictionDto.Name };

            await _restrictionRepository.AddRestrictionAsync(newRestriction);
            return Created("", newRestriction);
        }
        
        [HttpGet]
        [ActionName("GetAllRestrictions")]
        public async Task<ActionResult<IEnumerable<RestrictionDto>>> GetAllRestrictionsAsync()
        {
            var restrictions = await _restrictionRepository.GetAllRestrictionsAsync();
        
            return Ok(restrictions);
        }

        [HttpGet("{id}")]
        [ActionName("GetRestrictionById")]
        public async Task<ActionResult<RestrictionDto>> GetRestrictionByIdAsync(int id)
        {
            var restriction = await _restrictionRepository.GetRestrictionByIdAsync(id);

            if (restriction == null)
                return NotFound($"Restriction with Id {id} cannot found");

            return Ok(restriction);
        }
                
        [HttpPut("{id}")]
        [ActionName("UpdateRestriction")]
        public async Task<ActionResult<RestrictionDto>> UpdateRestrictionAsync(int id, RestrictionDto restrictionDto)
        {
            var existingRestriction = await _restrictionRepository.GetRestrictionByIdAsync(id);
            if (existingRestriction == null)
                return NotFound($"Restriction with Id {id} cannot be found");

            if (await _restrictionRepository.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                return BadRequest($"A restriction with the Name {restrictionDto.Name} already Exists");

            existingRestriction.Name = restrictionDto.Name;

            var updatedRestrictionDto = new Restriction
            {
                Id = existingRestriction.Id,
                Name = restrictionDto.Name,
            };

            await _restrictionRepository.UpdateRestrictionAsync(updatedRestrictionDto);
            return Ok(existingRestriction);
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteRestriction")]
        public async Task<ActionResult> DeleteRestrictionAsync(int id)
        {
            var restriction = await _restrictionRepository.GetRestrictionByIdAsync(id);
            if (restriction == null)
                return NotFound($"restriction with Id {id} does not Exist");

            var restrictionHasIngredientRelations = await _ingredientRestrictionRepository.AnyIngredientWithRestrictionAsync(restriction.Id); 
            if (restrictionHasIngredientRelations)
                return BadRequest($"restriction with Id {id} has existing Relations to Ingredients, cannot delete");
            
            var restrictionHasRecipeRelations = await _recipeRestrictionRepository.AnyRecipeWithRestrictionAsync(id);
            if (restrictionHasRecipeRelations)
                return BadRequest($"restriction with Id {id} has existing Relations to Recipes, cannot delete");
            
            //var restrictionHasIngredientRelations = await ingredientRepository.AnyIngredientWithRestrictionAsync(id); //TODO remove Method
            //var restrictionHasRecipeRelations = await _recipeRepository.AnyRecipesWithRestrictionAsync(id); //TODO remove Method
            await _restrictionRepository.DeleteRestrictionAsync(id);

            return NoContent();
        }

    }
}
