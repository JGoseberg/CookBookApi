using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitController : ControllerBase
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly IMapper _mapper;
        public MeasurementUnitController(IMeasurementUnitRepository measurementUnitRepository, IMapper mapper)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddMeasurementUnitAsync(MeasurementUnitDto measurementUnitDto)
        {
            // TODO Add specific DTO

            if (await _measurementUnitRepository.AnyMeasurementUnitWithSameNameAsync(measurementUnitDto.Name))
                return BadRequest("A MeasurementUnit with this name already exists");

            var newMeasurementUnit = new MeasurementUnit { Name = measurementUnitDto.Name, Abbreviation = measurementUnitDto.Abbreviation };

            await _measurementUnitRepository.AddMeasurementUnitAsync(newMeasurementUnit);
            return Ok(newMeasurementUnit);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMeasurementUnitAsync(int id)
        {
            var measurementUnit = await _measurementUnitRepository.GetMeasurementUnitByIdAsync(id);
            if (measurementUnit == null)
                return NotFound($"MeasurementUnit with {id} not found");

            await _measurementUnitRepository.DeleteMeasurementUnitAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeasurementUnitDto>>> GetAllMeasurementUnitsAsync()
        {
            var measurementUnits = await _measurementUnitRepository.GetAllMeasurementunitsAsync();

            return measurementUnits.ToList();
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
            if (id != measurementUnitDto.Id)
                return BadRequest("MeasurementUnitId missmatch");

            var updatedMeasurementUnit = new MeasurementUnit 
            { 
                Id = measurementUnitDto.Id,
                Name = measurementUnitDto.Name,
                Abbreviation = measurementUnitDto.Abbreviation
            };

            await _measurementUnitRepository.UpdateMeasurementUnitAsync(updatedMeasurementUnit);

            return Ok(updatedMeasurementUnit);
        }
    }
}
