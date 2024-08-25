using CookBookApi.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Ingredient> Ingredients { get; set; }

    // Add this collection to allow a recipe to reference other recipes
    public ICollection<Recipe> Subrecipes { get; set; }

    // Add this to keep track of parent recipes
    public ICollection<Recipe> ParentRecipes { get; set; } // Recipes that include this as a subrecipe
}