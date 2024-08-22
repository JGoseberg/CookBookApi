namespace CookBookApi.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int AmountId { get; set; }
        public Amount Amount { get; set; }
    }
}
