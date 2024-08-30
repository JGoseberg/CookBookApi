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
        private readonly ICuisineRepository _cuisineRepository;
        private readonly IMapper _mapper;

        public CuisinesController(ICuisineRepository cuisineRepository, IMapper mapper)
        {
            _cuisineRepository = cuisineRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddCuisineAsync(CuisineDto cuisineDto)
        {
            var newCuisine = new Cuisine { Name = cuisineDto.Name };

            await _cuisineRepository.AddCuisineAsync(newCuisine);

            return Ok(newCuisine);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuisineDto>>> GetCuisinesAsync()
        {
            var cuisines = await _cuisineRepository.GetAllCuisinesAsync();

            return cuisines.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cuisine>> GetCuisineById(int id)
        {
            var cuisine = await _cuisineRepository.GetCuisineByIdAsync(id);

            if (cuisine == null)
                return null;
            return Ok(cuisine);
        }
                
        [HttpPut("{id}")]
        public async Task<ActionResult<CuisineDto>> UpdateCuisineAsync(int id, CuisineDto cuisineDto)
        {
            var updatedCuisine = new Cuisine { Id = id, Name = cuisineDto.Name };

            await _cuisineRepository.UpdateCuisineAsync(updatedCuisine);

            return Ok(updatedCuisine);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCuisineAsync(int id)
        {
            // TODO FK_Constraint_Recipes
            var cuisine = await _cuisineRepository.GetCuisineByIdAsync(id);
            if (cuisine == null)
                return NotFound($"Cuisine with {id} not found");

            var hasRelatedRecipes = await _

            await _cuisineRepository.DeleteCuisineAsync(id);

            return NoContent();
        }
    }
}
