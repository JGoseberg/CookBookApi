using CookBookApi.Models;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instruction { get; set; }
    public string Creator { get; set; }
    public DateTime CreateTime { get; set; }
    public int CuisineId { get; set; } // Foreign Key
    public Cuisine Cuisine { get; set; } // Navigation property
    public List<RecipeIngredient> RecipeIngredients { get; set; }
    public List<Recipe> Subrecipes { get; set; }
    public int? ParentRecipeId { get; set; }
    public List<Recipe> ParentRecipes { get; set; }
    public List<RecipeRestriction> RecipeRestrictions { get; set; }
}
