using CookBookApi.Models;
using CookBookApi.Enums;

namespace CookBookApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CookBookContext context)
        {
            if (context.Recipes.Any())
                return;

            var ingredients = new Ingredient[]
            {
                new Ingredient {Name = "Nudeln", Description = "Lorem Ipsum"},
                new Ingredient {Name = "Bacon", Description = "Lorem Ipsum"},
            };
            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();


            var recipes = new Recipe[]
            {
                new Recipe 
                { 
                    Name = "Kräuterbutter",
                    Description = "Lorem Ipsum",
                    Type = RecipeType.Vegetarian,
                    Rating = RecipeRating.Excellent,
                    Creator = "Jonas"
                },
                new Recipe
                {
                    Name = "Schinkennudeln",
                    Description = "Lorem Ipsum",
                    Type = RecipeType.Normal,
                    Rating = RecipeRating.Good,
                    Creator = "Jonas",
                    Ingredients =
                    {
                        context.Ingredients.Where(x => x.Id == 1).First()
                    }
                }
            };
            context.Recipes.AddRange(recipes);
            context.SaveChanges();
        }
    }
}
