namespace CookBookApi.Models
{
    public class RecipeDietaryRestriction
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }


        public int DietaryRecipeRestrictionId { get; set; }
        public DietaryRestriction DietaryRestriction { get; set; }
    }
}
