using CookBookApi.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CuisineId { get; set; } // Foreign Key
    public Cuisine Cuisine { get; set; } // Navigation property
    public List<Ingredient> Ingredients { get; set; }
    public List<Recipe> Subrecipes { get; set; }
    public int? ParentRecipeId { get; set; }
    public List<Recipe> ParentRecipes { get; set; }
    public List<RecipeRestriction> RecipeRestrictions { get; set; }
}
