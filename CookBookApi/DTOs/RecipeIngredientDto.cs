namespace CookBookApi.DTOs
{
    public class RecipeIngredientDto
    {
        public required string IngredientName { get; set; }
        public required double Amount { get; set; }
        public required string MeasurementUnit { get; set; }
    }
}
