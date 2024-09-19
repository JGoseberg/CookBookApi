using AutoMapper;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public IngredientsController(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientDto>> GetIngredientByIdAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IngredientDto> AddIngredientAsync(IngredientDto ingredientDto)
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientDto);
            await _ingredientRepository.AddIngredientAsync(_mapper.Map<Ingredient>(ingredient));
            
            return _mapper.Map<IngredientDto>(ingredient);

        }

        [HttpPut("{id}")]
        public async Task UpdateIngredientAsync(int id, Ingredient ingredient)
        {
            await _ingredientRepository.UpdateIngredientAsync(ingredient);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIngredientAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
