using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;

namespace CookBookApi.Services;

public class RecipeImageService : IRecipeImageService
{
    private const long FileSize = 5 * 1024 * 1024; // MB * KB * B
    private readonly HashSet<string> _allowedMimeTypes = ["image/jpeg", "image/png", "image/webp"];
    private readonly IRecipeImageRepository _repository;

    public RecipeImageService(IRecipeImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<RecipeImage?> ProcessAndCreateRecipeImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;
        
        if (file.Length > FileSize)
            return null;
        
        if (!_allowedMimeTypes.Contains(file.ContentType.ToLower()))
            return null;
        var mimeType = file.ContentType;
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var imageData = memoryStream.ToArray();

        var existingImage = await _repository.GetExistingImageAsync(imageData, mimeType);
        
        if (existingImage != null)
            return existingImage;
        
        var base64 = Convert.ToBase64String(imageData);

        return new RecipeImage
        {
            ImageData = imageData,
            MimeType = mimeType
        };
    }
}