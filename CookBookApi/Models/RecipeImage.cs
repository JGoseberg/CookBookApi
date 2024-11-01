namespace CookBookApi.Models;

public class RecipeImage
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public byte[] ImageData { get; set; }
    public string MimeType { get; set; }
    public Recipe Recipe { get; set; }
}