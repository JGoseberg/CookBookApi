namespace CookBookApi.DTOs
{
    public interface IMeasurementUnits
    {
        Task<RecipeDto> GetMeasurementUnitByIdAsync(int id);
        Task<IEnumerable<RecipeDto>> GetAllMeasurementUnitsAsync();
        Task AddMeasurementUnitAsync(MeasurementUnit measurementUnit);
        Task UpdateMeasurementUnitAsync(MeasurementUnit measurementUnit);
        Task DeleteMeasurementUnitAsync(int id);
    }
}
