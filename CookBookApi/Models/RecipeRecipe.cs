namespace CookBookApi.Models
{
    public class RecipeRecipe
    {
        public int ParentRecipeId { get; set; }
        public Recipe ParentRecipe { get; set; }

        public int ChildRecipeId {  get; set; }
        public Recipe ChildRecipe { get; set; }
    }
}
