namespace CookBookApi.Models
{
    public class Measurement
    {
        public int MeasurementId { get; set; }
        public decimal Amount { get; set; }

        public int MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }


        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
