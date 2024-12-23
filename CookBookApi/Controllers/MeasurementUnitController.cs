﻿using AutoMapper;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeasurementUnitController : ControllerBase
    {
        private readonly IMeasurementUnitRepository _measurementUnitRepository;
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;
        
        public MeasurementUnitController(
            IMeasurementUnitRepository measurementUnitRepository,
            IRecipeIngredientRepository recipeIngredientRepository,
            IMapper mapper
            )
        {
            _measurementUnitRepository = measurementUnitRepository;
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }
        [HttpPost]
        [ActionName("AddMeasurementUnit")]
        public async Task<ActionResult> AddMeasurementUnitAsync(AddMeasurementUnitDto addMeasurementUnitDto)
        {
            if (addMeasurementUnitDto.Name.IsNullOrEmpty())
                return BadRequest("Name cannot be empty");

            if (addMeasurementUnitDto.Abbreviation.IsNullOrEmpty())
                return BadRequest("Abbreviation cannot be empty");

            if (await _measurementUnitRepository.AnyMeasurementUnitWithSameNameAsync(addMeasurementUnitDto.Name))
                return BadRequest("A MeasurementUnit with this name already exists");

            var newMeasurementUnit = new MeasurementUnit { Name = addMeasurementUnitDto.Name, Abbreviation = addMeasurementUnitDto.Abbreviation };

            await _measurementUnitRepository.AddMeasurementUnitAsync(newMeasurementUnit);

            return Created("", newMeasurementUnit);
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteMeasurementUnit")]
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
        [ActionName("GetMeasurementUnits")]
        public async Task<ActionResult<IEnumerable<MeasurementUnitDto>>> GetAllMeasurementUnitsAsync()
        {
            var measurementUnits = await _measurementUnitRepository.GetAllMeasurementunitsAsync();

            return Ok(measurementUnits);
        }

        [HttpGet("{id}")]
        [ActionName("GetMeasurementUnitById")]
        public async Task<ActionResult<MeasurementUnitDto>> GetMeasurementUnitByIdAsync(int id)
        {
            var measurementUnit = await _measurementUnitRepository.GetMeasurementUnitByIdAsync(id);

            if (measurementUnit == null)
                return NotFound();

            return Ok(measurementUnit);
        }

        [HttpPut("{id}")]
        [ActionName("UpdateMeasurementUnit")]
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
