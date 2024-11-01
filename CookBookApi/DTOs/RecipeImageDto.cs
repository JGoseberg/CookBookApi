namespace CookBookApi.DTOs;

public class RecipeImageDto
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public string ImageData { get; set; }
    public string MimeType { get; set; }
}