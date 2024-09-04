using AutoMapper;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories
{
    public class MeasurementunitRepository : IMeasurementUnitRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;
        public MeasurementunitRepository(CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MeasurementUnit> AddMeasurementUnitAsync(MeasurementUnit measurementUnit)
        {
            var addedEntity = await _context.MeasurementUnits.AddAsync(measurementUnit);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<bool> AnyMeasurementUnitWithSameNameAsync(string name)
        {
            return await _context.MeasurementUnits.AnyAsync(m => m.Name == name);
        }

        public async Task DeleteMeasurementUnitAsync(int id)
        {
            var measurementUnitToDelete = await _context.MeasurementUnits.FindAsync(id);

            if (measurementUnitToDelete == null)
                return;

            _context.MeasurementUnits.Remove(measurementUnitToDelete);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<MeasurementUnitDto>> GetAllMeasurementunitsAsync()
        {
            var measurementUnits = await _context.MeasurementUnits.ToListAsync();

            return _mapper.Map<IEnumerable<MeasurementUnitDto>>(measurementUnits);
        }

        public async Task<MeasurementUnitDto> GetMeasurementUnitByIdAsync(int id)
        {
            var measurementUnit = await _context.MeasurementUnits.FirstOrDefaultAsync(m => m.Id == id);

            if (measurementUnit == null)
                return null;

            return _mapper.Map<MeasurementUnitDto>(measurementUnit);
        }

        public async Task UpdateMeasurementUnitAsync(MeasurementUnit measurementUnit)
        {
            //  TODO does not update correcty instead add a new one
            var existingMeasurementUnit = await _context.MeasurementUnits.FindAsync(measurementUnit.Id);

            existingMeasurementUnit.Name = measurementUnit.Name;
            existingMeasurementUnit.Abbreviation = measurementUnit.Abbreviation;

            _context.MeasurementUnits.Update(existingMeasurementUnit);
            await _context.SaveChangesAsync();
        }
    }
}
