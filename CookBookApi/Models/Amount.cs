using CookBookApi.Enums;

namespace CookBookApi.Models
{
    public class Amount
    {
        public int AmountId { get; set; }
        public double Quantity { get; set; }
        public MeasurementUnitEnum MeasurementUnit { get; set; }
        //public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        //public ICollection<RecipeSubRecipe> RecipeSubRecipes { get; set; }

    }
}
