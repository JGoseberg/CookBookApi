namespace CookBookApi.DTOs.Recipes
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
        public List<RecipeDto> Subrecipes { get; set; }
        public List<RecipeDto> ParentRecipes { get; set; }
        public CuisineDto Cuisine { get; set; }
        public List<RestrictionDto> Restrictions { get; set; }
    }
}
