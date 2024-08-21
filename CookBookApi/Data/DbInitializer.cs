using CookBookApi.Models;

namespace CookBookApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CookBookContext context)
        {
            if (context.Recipes.Any())
                return;

            var recipes = new Recipe[]
            {
                new Recipe
                {
                    Name = "Schinkennudeln",
                    Description = "Lorem Ipsum",
                    Type = Enums.RecipeType.Normal,
                    Rating = 4,
                    Creator = "Jonas",
                }
            };
            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            var ingredients = new Ingredient[]
            {
                new Ingredient {Name = "Nudeln", Description = "Lorem Ipsum"},
                new Ingredient {Name = "Bacon", Description = "Lorem Ipsum"},
            };
            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var recipeIngredient = new RecipeIngredient[]
            {
                new RecipeIngredient{RecipeId = 1, IngredientId = 1},
                new RecipeIngredient{RecipeId = 1, IngredientId = 2}
            };
            context.RecipeIngredients.AddRange(recipeIngredient);
            context.SaveChanges();
        }
    }
}
