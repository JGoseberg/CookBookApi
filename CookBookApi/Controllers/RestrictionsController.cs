using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestrictionsController : ControllerBase
    {
        private readonly IRestrictionRepository _restrictionRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RestrictionsController
            (
            IRestrictionRepository restrictionRepository,
            IIngredientRepository ingredientRepository,
            IRecipeRepository recipeRepository,
            IMapper mapper
            )
        {
            _restrictionRepository = restrictionRepository;
            _ingredientRepository = ingredientRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        [HttpPost]
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
        public async Task<ActionResult<IEnumerable<RestrictionDto>>> GetAllRestrictionsAsync()
        {
            var restrictions = await _restrictionRepository.GetAllRestrictionsAsync();
        
            return Ok(restrictions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestrictionDto>> GetRestrictionByIdAsync(int id)
        {
            var restriction = await _restrictionRepository.GetRestrictionByIdAsync(id);

            if (restriction == null)
                return NotFound($"Restriction with Id {id} cannot found");

            return Ok(restriction);
        }
                
        [HttpPut("{id}")]
        public async Task<ActionResult<RestrictionDto>> UpdateRestrictionAsync(int id, RestrictionDto restrictionDto)
        {
            var existingRestriction = await _restrictionRepository.GetRestrictionByIdAsync(id);
            if (existingRestriction == null)
                return NotFound($"Restriction with Id {id} cannot be found");

            var restrictionWithSameName = await _restrictionRepository.AnyRestrictionWithSameNameAsync(restrictionDto.Name);
            if (restrictionWithSameName == true)
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
        public async Task<ActionResult> DeleteRestrictionAsync(int id)
        {
            var restrictionExists = await _restrictionRepository.GetRestrictionByIdAsync(id);
            if (restrictionExists == null)
                return NotFound($"restriction with Id {id} does not Exist");

            var restrictionHasIngredientRelations = await _ingredientRepository.AnyIngredientWithRestrictionAsync(id);
            if (restrictionHasIngredientRelations)
                return BadRequest($"restriction with Id {id} has existing Relations to Ingredients, cannot delete");

            var restrictionHasRecipeRelations = await _recipeRepository.AnyRecipesWithRestrictionAsync(id);
            if (restrictionHasRecipeRelations)
                return BadRequest($"restriction with Id {id} has existing Relations to Recipes, cannot delete");

            await _restrictionRepository.DeleteRestrictionAsync(id);

            return NoContent();
        }

    }
}
