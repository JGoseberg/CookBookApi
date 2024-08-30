using CookBookApi.DTOs;

namespace CookBookApi.Interfaces.Repositories
{
    public interface IMeasurementUnitRepository
    {
        Task<bool> AnyMeasurementUnitWithSameNameAsync(string name);
        Task AddMeasurementUnitAsync(MeasurementUnit measurementUnit);
        Task DeleteMeasurementUnitAsync(int id);
        Task<IEnumerable<MeasurementUnitDto>> GetAllMeasurementunitsAsync();
        Task<MeasurementUnitDto> GetMeasurementUnitByIdAsync(int id);
        Task UpdateMeasurementUnitAsync(MeasurementUnit measurementUnit);
    }
}
