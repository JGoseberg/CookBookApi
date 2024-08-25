namespace CookBookApi.Models
{
    public class Restriction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<RecipeRestriction> RecipeRestrictions { get; set; }
        public ICollection<IngredientRestriction> IngredientRestrictions { get; set; }
    }
}
