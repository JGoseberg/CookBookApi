namespace CookBookApi.Models
{
    public class SubRecipe
    {
        public int SubRecipeId { get; set; }
        public ICollection<RecipeSubRecipe> RecipeSubRecipes { get; set; }
    }
}
