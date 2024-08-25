using CookBookApi.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public int MeasurementUnitId { get; set; }
    public MeasurementUnit MeasurementUnit { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public int CuisineId { get; set; } // Foreign Key
    public Cuisine Cuisine { get; set; } // Navigation property
}

