namespace CookBookApi.Models
{
    public class Restriction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RecipeRestriction> RecipeRestrictions { get; set; }
        public ICollection<IngredientRestriction> IngredientRestrictions { get; set; }
    }
}
