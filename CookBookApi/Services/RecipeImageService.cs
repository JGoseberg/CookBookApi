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

    public async Task<RecipeImage> ProcessAndCreateRecipeImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid File");
        
        if (file.Length > FileSize)
            throw new ArgumentException("File is too large. Please use only Files smaller than 5 MB");
        
        if (!_allowedMimeTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException("Unsupported File type Allowed are only jpg, jpeg, png, webp");
        var mimeType = file.ContentType;
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var imageData = memoryStream.ToArray();

        if (await _repository.ImageExistsAsync(imageData, mimeType))
            throw new InvalidOperationException("Image already exists");
        
        var base64 = Convert.ToBase64String(imageData);

        return new RecipeImage
        {
            ImageData = imageData,
            MimeType = mimeType
        };
    }
}