namespace CookBookApi.Models
{
    public class Cuisine
    {
        // multiple Times of one Cuisine

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }

}
