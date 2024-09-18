using AutoMapper;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitController : ControllerBase
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public MeasurementUnitController
            (
            IMeasurementUnitRepository measurementUnitRepository,
            IRecipeIngredientRepository recipeIngredientRepository , 
            IMapper mapper
            )
        {
            _measurementUnitRepository = measurementUnitRepository;
            _mapper = mapper;
            _recipeIngredientRepository = recipeIngredientRepository;
        }

        [HttpPost]
        public async Task<ActionResult> AddMeasurementUnitAsync(AddMeasurementUnitDto AddMeasurementUnitDto)
        {
            if (AddMeasurementUnitDto.Name.IsNullOrEmpty())
                return BadRequest("Name cannot be empty");

            if (AddMeasurementUnitDto.Abbreviation.IsNullOrEmpty())
                return BadRequest("Abbreviation cannot be empty");

            if (await _measurementUnitRepository.AnyMeasurementUnitWithSameNameAsync(AddMeasurementUnitDto.Name))
                return BadRequest("A MeasurementUnit with this name already exists");

            var newMeasurementUnit = new MeasurementUnit { Name = AddMeasurementUnitDto.Name, Abbreviation = AddMeasurementUnitDto.Abbreviation };

            await _measurementUnitRepository.AddMeasurementUnitAsync(newMeasurementUnit);

            return Created("", newMeasurementUnit);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMeasurementUnitAsync(int id)
        {
            var measurementUnit = await _measurementUnitRepository.GetMeasurementUnitByIdAsync(id);
            if (measurementUnit == null)
                return NotFound($"MeasurementUnit with {id} not found");

            if (await _recipeIngredientRepository.AnyRecipesWithMeasurementUnitAsync(id))
                return BadRequest("Cannot delete MeasurementUnit, it is used by a recipe");

            await _measurementUnitRepository.DeleteMeasurementUnitAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeasurementUnitDto>>> GetAllMeasurementUnitsAsync()
        {
            var measurementUnits = await _measurementUnitRepository.GetAllMeasurementunitsAsync();

            return Ok(measurementUnits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeasurementUnitDto>> GetMeasurementUnitByIdAsync(int id)
        {
            var measurementUnit = await _measurementUnitRepository.GetMeasurementUnitByIdAsync(id);

            if (measurementUnit == null)
                return NotFound();

            return Ok(measurementUnit);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MeasurementUnitDto>> UpdateMeasurementUnitAsync(int id, MeasurementUnitDto measurementUnitDto)
        {
            var existingMeasurementUnit = await _measurementUnitRepository.GetMeasurementUnitByIdAsync(id);

            if (existingMeasurementUnit == null)
                return NotFound($"MeasurementUnit with Id {id} not found");

            if (await _measurementUnitRepository.AnyMeasurementUnitWithSameNameAsync(measurementUnitDto.Name))
                return BadRequest("A MeasurementUnit with this already exists.");

            var updatedMeasurementUnit = new MeasurementUnit 
            { 
                Id = measurementUnitDto.Id,
                Name = measurementUnitDto.Name,
                Abbreviation = measurementUnitDto.Abbreviation
            };

            await _measurementUnitRepository.UpdateMeasurementUnitAsync(updatedMeasurementUnit);

            var updatedMeasurementUnitDto = _mapper.Map<MeasurementUnitDto>(updatedMeasurementUnit);

            return Ok(updatedMeasurementUnitDto);
        }
    }
}
