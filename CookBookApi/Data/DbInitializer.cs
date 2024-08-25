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
                    Name = "Pancakes",
                    Description = "Fluffy pancakes with syrup",
                    Ingredients = new Ingredient[]
                    {
                        new Ingredient { Name = "Flour" },
                        new Ingredient { Name = "Milk" },
                        new Ingredient { Name = "Eggs" },
                        new Ingredient { Name = "Flour" },
                    }
                },
                new Recipe
                {
                    Name = "Cookies",
                    Description = "Chocolate chip cookies",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Flour" },
                        new Ingredient { Name = "Sugar" },
                        new Ingredient { Name = "Butter" },
                        new Ingredient { Name = "Chocolate Chips" }
                    }
                },
                new Recipe
                {
                    Name = "Chocolate Sauce",
                    Description = "Melt chocolate and mix with cream",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Chocolate" },
                        new Ingredient { Name = "Cream" }
                    }
                }
            };
            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            // Now associate Chocolate Sauce as a subrecipe of Cookies
            recipes[1].Subrecipes = new List<Recipe> { recipes[2] };
            context.SaveChanges();

            var subrecipes = new Subrecipe[]
            {
                new Subrecipe
                {
                    Name = "Chocolate Sauce",
                    Instructions = "Melt chocolate and mix with cream",
                    RecipeId = recipes[1].Id // Assign to Cookies recipe
                }
            };
            context.Subrecipes.AddRange(subrecipes);
            context.SaveChanges();
        }
    }
}