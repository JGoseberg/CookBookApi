using CookBookApi.Enums;

namespace CookBookApi.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Title { get; set; }
        //public DietEnum Diet { get; set; }
        //public string? Notes { get; set; }
        //public DateTime CreatedAtDateTimeCreated { get; set; }

        //public int CountryId { get; set; }
        //public Country Countries { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
