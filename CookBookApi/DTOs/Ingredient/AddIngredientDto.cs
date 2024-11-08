namespace CookBookApi.DTOs.Ingredient
{
    public class AddIngredientDto
    {
        public required string Name { get; set; }

        public int CuisineId { get; set; }
    }
}
