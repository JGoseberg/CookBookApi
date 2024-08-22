namespace CookBookApi.Models
{
    public class MeasurementUnit
    {
        public int MeasurementUnitId { get; set; }
        public string MeasurementUnitName { get; set; }


        public ICollection<Measurement> Measurements { get; set; }
    }
}
