public static class DbInitializer
{
    public static void Initialize(CookBookContext context)
    {
        context.Database.EnsureCreated();

        if (context.Recipes.Any())
        {
            return;   // DB has been seeded
        }

        // Seed measurement units
        var units = new MeasurementUnit[]
        {
            new MeasurementUnit { Name = "Teaspoon", Abbreviation = "tsp" },
            new MeasurementUnit { Name = "Cup", Abbreviation = "cup" },
            new MeasurementUnit { Name = "Piece", Abbreviation = "pcs" }
        };

        context.MeasurementUnits.AddRange(units);
        context.SaveChanges();

        // Seed main recipes
        var pancakes = new Recipe
        {
            Name = "Pancakes",
            Description = "Fluffy pancakes with syrup",
            Ingredients = new List<Ingredient>()
        };

        var cookies = new Recipe
        {
            Name = "Cookies",
            Description = "Chocolate chip cookies",
            Ingredients = new List<Ingredient>()
        };

        var chocolateSauce = new Recipe
        {
            Name = "Chocolate Sauce",
            Description = "Melt chocolate and mix with cream",
            Ingredients = new List<Ingredient>()
        };

        // Adding the standalone recipes
        context.Recipes.AddRange(pancakes, cookies, chocolateSauce);
        context.SaveChanges();

        // Adding ingredients with Amount and MeasurementUnit
        var ingredients = new Ingredient[]
        {
            new Ingredient { Name = "Salt", Amount = 0.5, MeasurementUnitId = units[0].Id, RecipeId = pancakes.Id },
            new Ingredient { Name = "Sugar", Amount = 1, MeasurementUnitId = units[1].Id, RecipeId = cookies.Id },
            new Ingredient { Name = "Flour", Amount = 2, MeasurementUnitId = units[1].Id, RecipeId = pancakes.Id },
            new Ingredient { Name = "Butter", Amount = 0.5, MeasurementUnitId = units[1].Id, RecipeId = cookies.Id },
            new Ingredient { Name = "Milk", Amount = 1, MeasurementUnitId = units[1].Id, RecipeId = pancakes.Id },
            new Ingredient { Name = "Eggs", Amount = 2, MeasurementUnitId = units[2].Id, RecipeId = pancakes.Id },
            new Ingredient { Name = "Chocolate Chips", Amount = 1.5, MeasurementUnitId = units[1].Id, RecipeId = cookies.Id }
        };

        context.Ingredients.AddRange(ingredients);
        context.SaveChanges();

        // Assign Chocolate Sauce as a subrecipe of Cookies
        cookies.Subrecipes = new List<Recipe> { chocolateSauce };
        context.SaveChanges();
    }
}
