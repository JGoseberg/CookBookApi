namespace CookBookApi.Models
{
    public class IngredientCountry
    {
        public int IngredientID { get; set; }
        public Ingredient Ingredient { get; set; }

        public int CountryKitchenId { get; set; }
        public CountryKitchen CountryKitchen { get; set; }
    }
}
