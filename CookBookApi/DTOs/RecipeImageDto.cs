namespace CookBookApi.DTOs;

public class RecipeImageDto
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public required string ImageData { get; set; }
    public required string MimeType { get; set; }
}