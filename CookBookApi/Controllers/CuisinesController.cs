using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace CookBookApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CuisinesController(
        ICuisineRepository cuisineRepository,
        IRecipeRepository recipeRepository,
        IMapper mapper)
        : ControllerBase
    {
        [HttpPost]
        [ActionName("AddCuisine")]
        public async Task<ActionResult> AddCuisineAsync(CuisineDto cuisineDto)
        {
            if (cuisineDto.Name.IsNullOrEmpty())
                return BadRequest($"Name: {cuisineDto.Name } cannot be empty");

            if (await cuisineRepository.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                return BadRequest("A Cuisine with this name already exists");

            var newCuisine = new Cuisine { Name = cuisineDto.Name };

            await cuisineRepository.AddCuisineAsync(newCuisine);

            return Created("", newCuisine);
        }
        
        [HttpDelete("{id}")]
        [ActionName("DeleteCuisine")]
        public async Task<ActionResult> DeleteCuisineAsync(int id)
        {
            var cuisine = await cuisineRepository.GetCuisineByIdAsync(id);
            if (cuisine == null)
                return NotFound($"Cuisine with id {id} not found");

            var hasRelatedRecipes = await recipeRepository.AnyRecipesWithCuisineAsync(id);
            if (hasRelatedRecipes)
                return BadRequest("Cannot delete cuisine, it is used by a recipe");

            await cuisineRepository.DeleteCuisineAsync(id);
            return NoContent();
        }
        
        [HttpGet]
        [ActionName("GetAllCuisines")]
        public async Task<ActionResult<IEnumerable<CuisineDto>>> GetCuisinesAsync()
        {
            var cuisines = await cuisineRepository.GetAllCuisinesAsync();

            return Ok(cuisines);
        }

        
        [HttpGet("{id}")]
        [ActionName("GetCuisineId")]
        public async Task<ActionResult<Cuisine>> GetCuisineByIdAsync(int id)
        {
            var cuisine = await cuisineRepository.GetCuisineByIdAsync(id);

            if (cuisine == null)
                return NotFound();

            var cuisineDto = mapper.Map<CuisineDto>(cuisine);
            return Ok(cuisineDto);
        }
                
        [HttpPut("{id}")]
        [ActionName("UpdateCuisine")]
        public async Task<ActionResult<CuisineDto>> UpdateCuisineAsync(int id, CuisineDto cuisineDto)
        {
            var existingCuisine = await cuisineRepository.GetCuisineByIdAsync(id);
            
            if (existingCuisine == null)
                return NotFound($"Cuisine with ID {id} not found.");

            if (await cuisineRepository.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                return BadRequest("A cuisine with this name already exists.");

            var updatedCuisine = new Cuisine { Id = id, Name = cuisineDto.Name };

            await cuisineRepository.UpdateCuisineAsync(updatedCuisine);

            var updatedCuisineDto = mapper.Map<CuisineDto>(updatedCuisine);
            return Ok(updatedCuisineDto);
        }
    }
}
