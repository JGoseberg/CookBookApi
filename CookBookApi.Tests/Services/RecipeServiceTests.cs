using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Services;
using Moq;

namespace CookBookApi.Tests.Services;

[TestFixture]
public class RecipeServiceTests
{
    private RecipeService _recipeService;
    private Mock<IRecipeRepository> _recipeRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _recipeService = new RecipeService(_recipeRepositoryMock.Object);
    }

    [Test]
    public async Task GetRandomRecipeAsync_ShouldReturnRandomRecipe()
    {
        var recipeDtos = new List<RecipeDto>
        {
            new RecipeDto(),
            new RecipeDto(),
        };

        _recipeRepositoryMock.Setup(x => x.GetAllRecipesAsync())
            .ReturnsAsync(recipeDtos);
        
        var result = await _recipeService.GetRandomRecipeAsync();
        
        Assert.That(result, Is.InstanceOf<RecipeDto>());
    }
}
