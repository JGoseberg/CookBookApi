using CookBookApi.Models;

namespace CookBookApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CookBookContext context)
        {
            if (context.Recipes.Any())
                return;

            var dietaryRestrictions = new DietaryRestriction[]
            {
                new DietaryRestriction { DietaryRestrictionName = "Vegan" },
                new DietaryRestriction { DietaryRestrictionName = "Vegetarisch" },
                new DietaryRestriction { DietaryRestrictionName = "Glutenfrei" },
            };

            context.DietaryRestrictions.AddRange(dietaryRestrictions);
            context.SaveChanges();

            var countries = new CountryKitchen[]
            {
                new CountryKitchen { CountryKitchenName = "Global" },
                new CountryKitchen { CountryKitchenName = "Deutsch" },
                new CountryKitchen { CountryKitchenName = "Italienisch" },
            };
            context.CountryKitchens.AddRange(countries);
            context.SaveChanges();

            var ingredients = new Ingredient[]
            {
                new Ingredient { IngredientName = "Mehl" },
                new Ingredient { IngredientName = "Zucker" },
                new Ingredient { IngredientName = "Ei(er)" },
                new Ingredient { IngredientName = "Milch" },
                new Ingredient { IngredientName = "Zwiebel" },
            };
            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var ingredientCountries = new IngredientCountry[]
            {
                new IngredientCountry { IngredientID = ingredients[0].IngredientId, CountryKitchenId = countries[0].CountryKitchenId },
                new IngredientCountry { IngredientID = ingredients[1].IngredientId, CountryKitchenId = countries[0].CountryKitchenId },
                new IngredientCountry { IngredientID = ingredients[2].IngredientId, CountryKitchenId = countries[0].CountryKitchenId },
                new IngredientCountry { IngredientID = ingredients[3].IngredientId, CountryKitchenId = countries[0].CountryKitchenId },
            };
            context.IngredientCountries.AddRange(ingredientCountries);
            context.SaveChanges();

            var ingredientDietaryRestrictions = new IngredientDietaryRestriction[]
            {
                new IngredientDietaryRestriction { IngredientsId = ingredients[0].IngredientId, DietaryRestrictionId = dietaryRestrictions[1].DietaryRestrictionId},
                new IngredientDietaryRestriction { IngredientsId = ingredients[1].IngredientId, DietaryRestrictionId = dietaryRestrictions[1].DietaryRestrictionId},
                new IngredientDietaryRestriction { IngredientsId = ingredients[2].IngredientId, DietaryRestrictionId = dietaryRestrictions[1].DietaryRestrictionId},
                new IngredientDietaryRestriction { IngredientsId = ingredients[3].IngredientId, DietaryRestrictionId = dietaryRestrictions[0].DietaryRestrictionId},
            };
            context.IngredientDietaryRestrictions.AddRange(ingredientDietaryRestrictions);
            context.SaveChanges();

            var measurementUnits = new MeasurementUnit[]
            {
                new MeasurementUnit { MeasurementUnitName = "Gramm"},
                new MeasurementUnit { MeasurementUnitName = "Stk"},
                new MeasurementUnit { MeasurementUnitName = "ml"},
            };
            context.MeasurementsUnits.AddRange(measurementUnits);
            context.SaveChanges();

            var measurements = new Measurement[]
            {
                new Measurement {Amount = 200, MeasurementUnitId = measurementUnits[0].MeasurementUnitId},
                new Measurement {Amount = 100, MeasurementUnitId = measurementUnits[0].MeasurementUnitId},
                new Measurement {Amount = 2, MeasurementUnitId = measurementUnits[1].MeasurementUnitId},
                new Measurement {Amount = 500, MeasurementUnitId = measurementUnits[2].MeasurementUnitId},
            };
            context.Measurements.AddRange(measurements);
            context.SaveChanges();

            var recipes = new Recipe[]
            {
                new Recipe { RecipeName = "Basiskuchen", Instruction = "Ein einfaches Basis-Kuchenrezept", Rating=4, CountryKitchen = "Global"},
                new Recipe { RecipeName = "Pfannkuchen", Instruction = "Ein leckeres veganes PfannkuchenRezept", Rating=5, CountryKitchen = "Global"},
            };
            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            var recipeDietaryRestriction = new RecipeDietaryRestriction[]
            {
                new RecipeDietaryRestriction {RecipeId = recipes[1].RecipeId, DietaryRecipeRestrictionId = dietaryRestrictions[0].DietaryRestrictionId }
            };
            context.RecipeDietaryRestrictions.AddRange(recipeDietaryRestriction);
            context.SaveChanges();

            var recipeIngredients = new RecipeIngredient[]
            {
                new RecipeIngredient { RecipeId = recipes[0].RecipeId, IngredientId = ingredients[0].IngredientId, MeasurementId = measurements[0].MeasurementId },
                new RecipeIngredient { RecipeId = recipes[0].RecipeId, IngredientId = ingredients[1].IngredientId, MeasurementId = measurements[1].MeasurementId },
                new RecipeIngredient { RecipeId = recipes[0].RecipeId, IngredientId = ingredients[2].IngredientId, MeasurementId = measurements[2].MeasurementId },
                new RecipeIngredient { RecipeId = recipes[1].RecipeId, IngredientId = ingredients[0].IngredientId, MeasurementId = measurements[0].MeasurementId },
                new RecipeIngredient { RecipeId = recipes[1].RecipeId, IngredientId = ingredients[3].IngredientId, MeasurementId = measurements[3].MeasurementId },
            };
            context.RecipeIngredients.AddRange(recipeIngredients);
            context.SaveChanges();

            var recipeRecipes = new RecipeRecipe[]
            {
                new RecipeRecipe { ParentRecipeId = recipes[0].RecipeId, ChildRecipeId = recipes[1].RecipeId },
            };
            context.RecipeRecipes.AddRange(recipeRecipes);
            context.SaveChanges();


        }
    }
}
