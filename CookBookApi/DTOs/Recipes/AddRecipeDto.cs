namespace CookBookApi.DTOs.Recipes
{
    public class AddRecipeDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Instruction { get; set; }
        public required string Creator { get; set; }
        public required List<RecipeIngredientDto> Ingredients { get; set; }
        public required List<RecipeDto> Subrecipes { get; set; }
        public required List<RecipeDto> ParentRecipes { get; set; }

        public int CuisineId { get; set; }
        public List<RestrictionDto>? Restrictions { get; set; }
    }
}
