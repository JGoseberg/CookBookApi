namespace CookBookApi.Models
{
    public class RecipeRestriction
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int RestrictionId { get; set; }
        public Restriction Restriction { get; set; }
    }
}
