namespace CookBookApi.Models
{
    public class Subrecipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
