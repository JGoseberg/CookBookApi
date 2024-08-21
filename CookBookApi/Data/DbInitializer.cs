using CookBookApi.Models;

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
                    Type = Enums.RecipeType.Vegetarian,
                    Rating = 4,
                    Creator = "Jonas"
                },
                new Recipe
                {
                    Name = "Schinkennudeln",
                    Description = "Lorem Ipsum",
                    Type = Enums.RecipeType.Normal,
                    Rating = 4,
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
