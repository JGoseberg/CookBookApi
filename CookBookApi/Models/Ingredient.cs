namespace CookBookApi.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }


        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<IngredientCountry> IngredientCountries { get; set; }
        public ICollection<IngredientDietaryRestriction> IngredientDietaryRestrictions { get; set; }
    }
}
