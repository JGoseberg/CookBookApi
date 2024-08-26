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

        public IngredientsController (IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();

            return Ok(ingredients);
        }

        [HttpPost]
        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            await _ingredientRepository.UpdateIngredientAsync(ingredient);
        }

        [HttpPut]
        public async Task<IngredientDto> AddIngredient(AddIngredientDto ingredientDto)
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientDto);
            await _ingredientRepository.AddIngredientAsync(_mapper.Map<Ingredient>(ingredient));
            
            return _mapper.Map<IngredientDto>(ingredient);

        }

    }
}
