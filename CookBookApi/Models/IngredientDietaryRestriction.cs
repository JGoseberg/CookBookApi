namespace CookBookApi.Models
{
    public class IngredientDietaryRestriction
    {
        public int IngredientsId { get; set; }
        public Ingredient Ingredient { get; set; }


        public int DietaryRestrictionId { get; set; }
        public DietaryRestriction DietaryRestriction { get; set; }
    }
}
