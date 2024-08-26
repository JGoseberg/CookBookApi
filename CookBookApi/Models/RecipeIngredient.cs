namespace CookBookApi.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }

        public double Amount { get; set; }
    }
}
