﻿using AutoMapper;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CookBookApi.Repositories
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly CookBookContext _context;
        private readonly IMapper _mapper;

        public RecipeIngredientRepository(CookBookContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<int>?> GetRecipesWithIngredientAsync(int ingredientId)
        {
            var recipes = await _context.RecipeIngredients.Where(ri => ri.IngredientId == ingredientId).ToListAsync();

            if (recipes.Count == 0)
                return null;
            
            var recipeIds = recipes.Select(ri => ri.RecipeId).Distinct();
            
            return recipeIds;
        }

        public async Task<IEnumerable<int>?> GetRecipesWithIngredientsAsync(List<int> ingredientIds)
        {
            if (ingredientIds.IsNullOrEmpty() || ingredientIds.Count < 2)
                return null;
            
            var recipeIds = await _context.RecipeIngredients
                .Where(ri => ingredientIds.Contains(ri.IngredientId))
                .GroupBy(ri => ri.RecipeId)
                .Where(group => group.Select(ri => ri.IngredientId).Distinct().Count() == ingredientIds.Count)
                .Select(group => group.Key)
                .ToListAsync();
            return recipeIds;
        }
        
        public async Task<bool> AnyRecipesWithIngredientAsync(int ingredientId)
        {
            return await _context.Recipes.AnyAsync(r => r.RecipeIngredients.Any(ri => ri.IngredientId == ingredientId));
        }
        
        public async Task<bool> AnyRecipesWithMeasurementUnitAsync(int measurementUnitIdId)
        {
            return await _context.RecipeIngredients.AnyAsync(x => x.MeasurementUnitId == measurementUnitIdId);
        }
    }
}
