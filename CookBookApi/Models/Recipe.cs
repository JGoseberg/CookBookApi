using CookBookApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public RecipeType Type { get; set; }
        public int Rating { get; set; }

        DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
