using CookBookApi.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; }
    public int CuisineId { get; set; } // Foreign Key
    public Cuisine Cuisine { get; set; } // Navigation property
    public List<IngredientRestriction> IngredientRestrictions { get; set; }

}

