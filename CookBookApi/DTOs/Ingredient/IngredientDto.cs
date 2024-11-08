namespace CookBookApi.DTOs.Ingredient
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public CuisineDto? Cuisine { get; set; }
        public List<RestrictionDto>? Restrictions { get; set; }
    }
}
