namespace CookBookApi.DTOs.Ingredient
{
    public class IngredientDto
    {
        // TODO cleanUp, Not needed to set an amount and Unit here
        public int Id { get; set; }
        public string Name { get; set; }
        public CuisineDto Cuisine { get; set; }
        public List<RestrictionDto> Restrictions { get; set; }
    }
}
