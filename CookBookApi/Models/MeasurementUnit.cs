using CookBookApi.Models;

public class MeasurementUnit
{
    public int Id { get; set; }
    public string Name { get; set; } // e.g., "teaspoon", "cup", "pieces"
    public string Abbreviation { get; set; } // e.g., "tsp", "cup", "pcs"

    // Navigation property
    public ICollection<Ingredient> Ingredients { get; set; }
}
