namespace CookBookApi.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
