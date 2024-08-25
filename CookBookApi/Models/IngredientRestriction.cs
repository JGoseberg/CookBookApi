namespace CookBookApi.Models
{
    public class IngredientRestriction
    {
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public int RestrictionId { get; set; }
        public Restriction Restriction { get; set; }
    }
}
