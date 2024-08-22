namespace CookBookApi.DTOs
{
    public class RecipeDetailDto
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeDescription { get; set;}
        public int Rating { get; set; }
        public string CountryKitchen { get; set; }


        public List<IngredientDetailDto> Ingredients { get; set;}
        public List<string> DietaryRestrictions { get; set; }
        public List<string> RelatedRecips { get; set; }
    }
}
