using CookBookApi.Controllers;
using CookBookApi.Interfaces.Repositories;
using Moq;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class RecipeIngredientControllerTests
    {
        private Mock<IRecipeIngredientRepository> _recipeIngredientRepositoryMock;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private Mock<IIngredientRepository> _ingredientRepositoryMock;
        private RecipeIngredientController _controller;

        [SetUp]
        public void Setup()
        {
            _recipeIngredientRepositoryMock = new Mock<IRecipeIngredientRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _controller = new RecipeIngredientController
                (
                _recipeIngredientRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _ingredientRepositoryMock.Object
                );
        }

        [Test]
        public async Task AddRecipeIngredientAsync_RecipeIsEmpty_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_RecipeIfNotExists()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_IngredientIsEmpty_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_AddIngredientIfNotExists()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_AmountIsEmpty_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_MeasurementUnitIsEmpty_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_AddMeasurementIfNotExists()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeIngredientAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task DeleteRecipeIngredientAsync_NotFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task DeleteRecipeIngredientAsync_ReturnsNoContent()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetAllRecipeIngredientsAsync_ReturnsOK()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeIngredientByIdAsync_ReturnsOK()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateRecipeIngredientAsync_IdNotFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateRecipeIngredientAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }
    }
}
