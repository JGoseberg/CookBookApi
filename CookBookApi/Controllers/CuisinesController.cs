using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;


namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisinesController : ControllerBase
    {
        //TODO change to StatusCode 201

        private readonly ICuisineRepository _cuisineRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public CuisinesController(ICuisineRepository cuisineRepository,IRecipeRepository recipeRepository, IMapper mapper)
        {
            _cuisineRepository = cuisineRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddCuisineAsync(CuisineDto cuisineDto)
        {
            if (await _cuisineRepository.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                // TODO BadRequest???
                return BadRequest("A Cuisine with this name already exists");

            var newCuisine = new Cuisine { Name = cuisineDto.Name };

            await _cuisineRepository.AddCuisineAsync(newCuisine);

            return CreatedAtAction(nameof(newCuisine), newCuisine);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCuisineAsync(int id)
        {
            var cuisine = await _cuisineRepository.GetCuisineByIdAsync(id);
            if (cuisine == null)
                return NotFound($"Cuisine with {id} not found");

            var hasRelatedRecipes = await _recipeRepository.AnyRecipesWithCuisineAsync(id);
            if (hasRelatedRecipes)
                return BadRequest("Cannot delete cuiseine, it is used by a recipe");

            await _cuisineRepository.DeleteCuisineAsync(id);
            return NoContent();
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuisineDto>>> GetCuisinesAsync()
        {
            var cuisines = await _cuisineRepository.GetAllCuisinesAsync();

            return Ok(cuisines.ToList());
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Cuisine>> GetCuisineById(int id)
        {
            var cuisine = await _cuisineRepository.GetCuisineByIdAsync(id);

            if (cuisine == null)
                return NotFound();
            return Ok(cuisine);
        }
                
        [HttpPut("{id}")]
        public async Task<ActionResult<CuisineDto>> UpdateCuisineAsync(int id, CuisineDto cuisineDto)
        {
            var updatedCuisine = new Cuisine { Id = id, Name = cuisineDto.Name };

            await _cuisineRepository.UpdateCuisineAsync(updatedCuisine);

            return Ok(updatedCuisine);
        }
    }
}
