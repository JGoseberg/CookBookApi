using CookBookApi.Models;

public static class DbInitializer
{
    public static void Initialize(CookBookContext context)
    {
        context.Database.EnsureCreated();

        if (context.Recipes.Any() || context.Ingredients.Any() || context.Restrictions.Any() || context.MeasurementUnits.Any())
        {
            return; // DB has been seeded
        }

        // Seed Measurement Units with Abbreviation
        var measurementUnits = new MeasurementUnit[]
        {
            new MeasurementUnit { Name = "grams", Abbreviation = "g" },
            new MeasurementUnit { Name = "cups", Abbreviation = "cups" },
            new MeasurementUnit { Name = "pieces", Abbreviation = "pcs" }
        };

        context.MeasurementUnits.AddRange(measurementUnits);
        context.SaveChanges();

        // Seed Cuisines
        var cuisines = new Cuisine[]
        {
            new Cuisine { Name = "Italian" },
            new Cuisine { Name = "American" }
        };

        context.Cuisines.AddRange(cuisines);
        context.SaveChanges();

        // Seed Restrictions
        var restrictions = new Restriction[]
        {
            new Restriction { Name = "Vegan" },
            new Restriction { Name = "Gluten-Free" },
            new Restriction { Name = "Nut-Free" }
        };

        context.Restrictions.AddRange(restrictions);
        context.SaveChanges();

        // Seed Recipes
        var recipes = new Recipe[]
        {
            new Recipe { Name = "Pancakes", Description = "Fluffy pancakes.", CuisineId = cuisines[1].Id },
            new Recipe { Name = "Cookies", Description = "Chocolate chip cookies.", CuisineId = cuisines[1].Id },
            new Recipe { Name = "Chocolate Sauce", Description = "Rich chocolate sauce.", CuisineId = cuisines[0].Id }
        };

        context.Recipes.AddRange(recipes);
        context.SaveChanges();

        // Seed Ingredients
        var ingredients = new Ingredient[]
        {
            new Ingredient { Name = "Salt", Amount = 1, MeasurementUnitId = measurementUnits[0].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Sugar", Amount = 2, MeasurementUnitId = measurementUnits[1].Id, RecipeId = recipes[1].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Flour", Amount = 3, MeasurementUnitId = measurementUnits[0].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Butter", Amount = 4, MeasurementUnitId = measurementUnits[0].Id, RecipeId = recipes[1].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Milk", Amount = 5, MeasurementUnitId = measurementUnits[0].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Eggs", Amount = 6, MeasurementUnitId = measurementUnits[2].Id, RecipeId = recipes[0].Id, CuisineId = cuisines[1].Id },
            new Ingredient { Name = "Chocolate Chips", Amount = 7, MeasurementUnitId = measurementUnits[1].Id, RecipeId = recipes[1].Id, CuisineId = cuisines[1].Id }
        };

        context.Ingredients.AddRange(ingredients);
        context.SaveChanges();

        // Seed Recipe Restrictions
        var recipeRestrictions = new RecipeRestriction[]
        {
            new RecipeRestriction { RecipeId = recipes[0].Id, RestrictionId = restrictions[0].Id }, // Pancakes: Vegan
            new RecipeRestriction { RecipeId = recipes[1].Id, RestrictionId = restrictions[1].Id }  // Cookies: Gluten-Free
        };

        context.RecipeRestrictions.AddRange(recipeRestrictions);
        context.SaveChanges();

        // Seed Ingredient Restrictions
        var ingredientRestrictions = new IngredientRestriction[]
        {
            new IngredientRestriction { IngredientId = ingredients[0].Id, RestrictionId = restrictions[0].Id }, // Salt: Vegan
            new IngredientRestriction { IngredientId = ingredients[1].Id, RestrictionId = restrictions[1].Id }  // Sugar: Gluten-Free
        };

        context.IngredientRestrictions.AddRange(ingredientRestrictions);
        context.SaveChanges();
    }
}
