namespace CookBookApi.Models
{
    public class RecipeSubRecipe
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int SubRecipeId { get; set; }
        public SubRecipe SubRecipe { get; set; }

        public int AmountId { get; set; }
        public Amount Amount { get; set; }
    }
}
