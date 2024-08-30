namespace CookBookApi.Models
{
    public class Cuisine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }

}
