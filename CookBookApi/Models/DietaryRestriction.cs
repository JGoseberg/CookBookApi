namespace CookBookApi.Models
{
    public class DietaryRestriction
    {
        public int DietaryRestrictionId { get; set; }
        public string DietaryRestrictionName { get; set; }


        public ICollection<RecipeDietaryRestriction> RecipeDietaryRestrictions { get; set; }
        public ICollection<IngredientDietaryRestriction> IngredientDietaryRestrictions { get; set; }
    }
}
