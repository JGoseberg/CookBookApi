using CookBookApi.Models;

public static class DbInitializer
{
    public static void Initialize(CookBookContext context)
    {
        context.Database.EnsureCreated();

        if (context.Recipes.Any() || context.Recipes.Any())
        {
            return;   // DB has been seeded
        }

        // Seed cuisines
        var cuisines = new Cuisine[]
        {
            new Cuisine { Name = "American" },
            new Cuisine { Name = "Italian" },
            new Cuisine { Name = "French" }
        };

        context.Cuisines.AddRange(cuisines);
        context.SaveChanges();

        // Seed recipes
        var recipes = new Recipe[]
        {
            new Recipe { Name = "Pancakes", Description = "Fluffy pancakes with syrup", CuisineId = cuisines[0].Id, Ingredients = new List<Ingredient>() },
            new Recipe { Name = "Cookies", Description = "Chocolate chip cookies", CuisineId = cuisines[1].Id, Ingredients = new List<Ingredient>() },
            new Recipe { Name = "Chocolate Sauce", Description = "Melt chocolate and mix with cream", CuisineId = cuisines[2].Id, Ingredients = new List<Ingredient>() }
        };

        context.Recipes.AddRange(recipes);
        context.SaveChanges();

        // Seed measurement units
        var units = new MeasurementUnit[]
        {
            new MeasurementUnit { Name = "Teaspoon", Abbreviation = "tsp" },
            new MeasurementUnit { Name = "Cup", Abbreviation = "cup" },
            new MeasurementUnit { Name = "Piece", Abbreviation = "pcs" }
        };

        context.MeasurementUnits.AddRange(units);
        context.SaveChanges();

        // Seed ingredients
        var ingredients = new Ingredient[]
        {
            new Ingredient { Name = "Salt", Amount = 0.5, MeasurementUnitId = units[0].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[0].Id },
            new Ingredient { Name = "Sugar", Amount = 1, MeasurementUnitId = units[1].Id, RecipeId = recipes[1].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Flour", Amount = 2, MeasurementUnitId = units[1].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[0].Id },
            new Ingredient { Name = "Butter", Amount = 0.5, MeasurementUnitId = units[1].Id, RecipeId = recipes[1].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Milk", Amount = 1, MeasurementUnitId = units[1].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[0].Id },
            new Ingredient { Name = "Eggs", Amount = 2, MeasurementUnitId = units[2].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[0].Id },
            new Ingredient { Name = "Chocolate Chips", Amount = 1.5, MeasurementUnitId = units[1].Id, RecipeId = recipes[1].Id, CuisineId = cuisines[1].Id }
        };

        context.Ingredients.AddRange(ingredients);
        context.SaveChanges();

        // Assign Chocolate Sauce as a subrecipe of Cookies
        recipes[1].Subrecipes = new List<Recipe> { recipes[2] };
        context.SaveChanges();
    }
}
