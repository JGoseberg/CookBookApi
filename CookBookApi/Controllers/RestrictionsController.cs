using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestrictionsController(
        IRestrictionRepository restrictionRepository,
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository)
        : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> AddRestrictionAsync(RestrictionDto restrictionDto)
        {
            if (restrictionDto.Name.IsNullOrEmpty())
                return BadRequest($"Name {restrictionDto.Name} cannot be empty");

            if (await restrictionRepository.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                return BadRequest($"A Restriction with the Name {restrictionDto.Name} already exists");

            var newRestriction = new Restriction { Name = restrictionDto.Name };

            await restrictionRepository.AddRestrictionAsync(newRestriction);
            return Created("", newRestriction);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestrictionDto>>> GetAllRestrictionsAsync()
        {
            var restrictions = await restrictionRepository.GetAllRestrictionsAsync();
        
            return Ok(restrictions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestrictionDto>> GetRestrictionByIdAsync(int id)
        {
            var restriction = await restrictionRepository.GetRestrictionByIdAsync(id);

            if (restriction == null)
                return NotFound($"Restriction with Id {id} cannot found");

            return Ok(restriction);
        }
                
        [HttpPut("{id}")]
        public async Task<ActionResult<RestrictionDto>> UpdateRestrictionAsync(int id, RestrictionDto restrictionDto)
        {
            var existingRestriction = await restrictionRepository.GetRestrictionByIdAsync(id);
            if (existingRestriction == null)
                return NotFound($"Restriction with Id {id} cannot be found");

            if (await restrictionRepository.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                return BadRequest($"A restriction with the Name {restrictionDto.Name} already Exists");

            existingRestriction.Name = restrictionDto.Name;

            var updatedRestrictionDto = new Restriction
            {
                Id = existingRestriction.Id,
                Name = restrictionDto.Name,
            };

            await restrictionRepository.UpdateRestrictionAsync(updatedRestrictionDto);
            return Ok(existingRestriction);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestrictionAsync(int id)
        {
            var restrictionExists = await restrictionRepository.GetRestrictionByIdAsync(id);
            if (restrictionExists == null)
                return NotFound($"restriction with Id {id} does not Exist");

            var restrictionHasIngredientRelations = await ingredientRepository.AnyIngredientWithRestrictionAsync(id);
            if (restrictionHasIngredientRelations)
                return BadRequest($"restriction with Id {id} has existing Relations to Ingredients, cannot delete");

            var restrictionHasRecipeRelations = await recipeRepository.AnyRecipesWithRestrictionAsync(id);
            if (restrictionHasRecipeRelations)
                return BadRequest($"restriction with Id {id} has existing Relations to Recipes, cannot delete");

            await restrictionRepository.DeleteRestrictionAsync(id);

            return NoContent();
        }

    }
}
