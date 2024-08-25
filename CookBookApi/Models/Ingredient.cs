public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; } // e.g., 2, 1.5, etc.

    // Foreign Key
    public int MeasurementUnitId { get; set; }
    public MeasurementUnit MeasurementUnit { get; set; }

    // Navigation properties
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}
