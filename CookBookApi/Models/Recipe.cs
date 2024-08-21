using CookBookApi.Enums;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CookBookApi.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string? Description { get; set; }
        
        public string? Creator { get; set; }
        
        public RecipeType? Type { get; set; }
        
        public int? Rating { get; set; }
        public List<Ingredient> Ingredients { get; } = [];
    }
}
