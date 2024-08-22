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

            var recipes = new Recipe[]
            {
                new Recipe
                {
                    Title = "Schinkennudeln"
                }
            };

            context.AddRange(recipes);
            context.SaveChanges();

            //var ingredientsOld = new Ingredient[]
            //{
            //    new Ingredient {Name = "Nudeln", Description = "Lorem Ipsum"},
            //    new Ingredient {Name = "Bacon", Description = "Lorem Ipsum"},
            //};
            //context.Ingredients.AddRange(ingredients);
            //context.SaveChanges();


            //var recipesOld = new Recipe[]
            //{
            //    new Recipe 
            //    { 
            //        Name = "Kräuterbutter",
            //        Description = "Lorem Ipsum",
            //        Type = RecipeType.Vegetarian,
            //        Rating = RecipeRatingEnum.Excellent,
            //        Creator = "Jonas"
            //    },
            //    new Recipe
            //    {
            //        Name = "Schinkennudeln",
            //        Description = "Lorem Ipsum",
            //        Type = RecipeType.Normal,
            //        Rating = RecipeRatingEnum.Good,
            //        Creator = "Jonas",
            //        Ingredients =
            //        {
            //            context.Ingredients.Where(x => x.Id == 1).First()
            //        }
            //    }
            //};
            //context.Recipes.AddRange(recipes);
            //context.SaveChanges();
        }
    }
}
