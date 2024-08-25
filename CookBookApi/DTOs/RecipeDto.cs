namespace CookBookApi.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
        public List<RecipeDto> Subrecipes { get; set; } = new List<RecipeDto>();
        public CuisineDto Cuisine { get; set; } 
    }
}
