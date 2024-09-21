namespace CookBookApi.DTOs.Recipes
{
    public class AddRecipeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public string Creator { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
        public List<RecipeDto> Subrecipes { get; set; }
        public List<RecipeDto> ParentRecipes { get; set; }

        public int CuisineId { get; set; }
        public List<RestrictionDto> Restrictions { get; set; }
    }
}
