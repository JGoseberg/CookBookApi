namespace CookBookApi.Models
{
    public class CountryKitchen
    {
        public int CountryKitchenId { get; set; }
        public string CountryKitchenName { get; set; }


        public ICollection<IngredientCountry> IngredientCountries { get; set; }
    }
}
