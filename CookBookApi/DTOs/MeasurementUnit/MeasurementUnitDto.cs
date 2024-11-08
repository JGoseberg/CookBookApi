namespace CookBookApi.DTOs.MeasurementUnit
{
    public class MeasurementUnitDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Abbreviation { get; set; }
    }
}
