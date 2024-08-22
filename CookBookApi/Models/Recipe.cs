using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CookBookApi.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string Instruction { get; set; }
        public int Rating { get; set; }
        public string? Creator { get; set; }
        public TimeOnly TimeToMake { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string CountryKitchen { get; set; }


        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<RecipeRecipe> ChildRecipes { get; set; }
        public ICollection<RecipeRecipe> ParentRecipes { get; set; }
        public ICollection<RecipeDietaryRestriction> RecipeDietaryRestrictions { get; set; }
    }
}
