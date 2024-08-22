using CookBookApi.Enums;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CookBookApi.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string? Instruction { get; set; }
        //public RecipeRatingEnum Rating { get; set; }
        //public string? Creator { get; set; }
        //public TimeOnly TimeToMake { get; set; }
        //public DateTime DateTimeCreated { get; set; }
        //public MealCategoryEnum MealCategory { get; set; }
        //public MealtimeEnum Mealtime { get; set; }

        //public int CountryId { get; set; }
        //public Country Countries { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<RecipeSubRecipe> RecipeSubRecipes { get; set; }
    }
}
