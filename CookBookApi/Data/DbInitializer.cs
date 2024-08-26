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
            new Recipe { Name = "Pancakes", Description = "Fluffy pancakes.", Instruction = "Steps", Creator = "ChatGPT", CreateTime = DateTime.Now, CuisineId = cuisines[1].Id },
            new Recipe { Name = "Cookies", Description = "Chocolate chip cookies.", Instruction = "Steps", Creator = "ChatGPT", CreateTime = DateTime.Now, CuisineId = cuisines[1].Id },
            new Recipe { Name = "Chocolate Sauce", Description = "Rich chocolate sauce.", Instruction = "Steps",Creator = "ChatGPT", CreateTime = DateTime.Now, CuisineId = cuisines[0].Id }
        };

        recipes[1].Subrecipes = new List<Recipe> { recipes[2] };

        context.Recipes.AddRange(recipes);
        context.SaveChanges();

        // Seed Ingredients
        var ingredients = new Ingredient[]
        {
            new Ingredient { Name = "Salt", CuisineId = cuisines[1].Id },
            new Ingredient {Name = "Sugar", CuisineId = cuisines[1].Id},
            new Ingredient {Name = "Flour", CuisineId = cuisines[1].Id},
            new Ingredient {Name = "Butter", CuisineId = cuisines[1].Id},
            new Ingredient {Name = "Milk", CuisineId = cuisines[1].Id},
            new Ingredient {Name = "Eggs", CuisineId = cuisines[1].Id},
            new Ingredient {Name = "Chocolate Chips", CuisineId = cuisines[1].Id}
        };

        context.Ingredients.AddRange(ingredients);
        context.SaveChanges();

        var recipeIngredients = new RecipeIngredient[]
        {
            new RecipeIngredient { Recipe = recipes[0], Ingredient = ingredients[0], Amount = 200, MeasurementUnit = measurementUnits[1] },
            new RecipeIngredient { Recipe = recipes[0], Ingredient = ingredients[1], Amount = 300, MeasurementUnit = measurementUnits[2] },

            new RecipeIngredient { Recipe = recipes[1], Ingredient = ingredients[1], Amount = 300, MeasurementUnit = measurementUnits[2] },
            new RecipeIngredient { Recipe = recipes[1], Ingredient = ingredients[2], Amount = 50, MeasurementUnit = measurementUnits[0] },
            new RecipeIngredient { Recipe = recipes[1], Ingredient = ingredients[3], Amount = 25, MeasurementUnit = measurementUnits[2] },

            new RecipeIngredient { Recipe = recipes[2], Ingredient = ingredients[1], Amount = 500, MeasurementUnit = measurementUnits[2] },
            new RecipeIngredient { Recipe = recipes[2], Ingredient = ingredients[5], Amount = 75, MeasurementUnit = measurementUnits[2] },
            new RecipeIngredient { Recipe = recipes[2], Ingredient = ingredients[3], Amount = 2, MeasurementUnit = measurementUnits[1] },
            new RecipeIngredient { Recipe = recipes[2], Ingredient = ingredients[4], Amount = 20, MeasurementUnit = measurementUnits[2] },

        };
        context.RecipeIngredients.AddRange(recipeIngredients);
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
