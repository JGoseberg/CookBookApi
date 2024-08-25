namespace CookBookApi.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<RecipeDto> Subrecipes { get; set; }
        public CuisineDto Cuisine { get; set; }
        public List<RestrictionDto> Restrictions { get; set; }
    }
}
