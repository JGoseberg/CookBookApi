using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Repositories;

public class RecipeImageRepository : IRecipeImageRepository
{
    private readonly CookBookContext _context;

    public RecipeImageRepository(CookBookContext context)
    {
        _context = context;
    }

    public async Task AddRecipeImageAsync(RecipeImage recipeImage)
    {
        _context.RecipeImages.Add(recipeImage);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RecipeImage>> GetRecipeImagesAsync(int recipeId)
    {
        return await _context.RecipeImages
            .Where(r => r.RecipeId == recipeId)
            .ToListAsync();
    }

    public async Task<bool> ImageExistsAsync(byte[] imageData, string mimeType)
    {
        return await _context.RecipeImages
            .AnyAsync((r) => r.MimeType == mimeType && r.ImageData == imageData);
    }
}