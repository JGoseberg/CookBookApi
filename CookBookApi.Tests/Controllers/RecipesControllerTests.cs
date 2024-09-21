using CookBookApi.Interfaces.Repositories;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class RecipesControllerTests
    {
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private RecipesController _recipeController;

        [SetUp]
        public void Setup()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _recipeController = new RecipesController(_recipeRepositoryMock.Object);
        }

        [Test]
        public async Task AddRecipeAsync_NameIsEmpty_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeAsync_InstructionIsEmpty_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeAsync_RecipeWithExactNameExists_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task AddRecipeAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task DeleteRecipeAsync_NotFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task DeleteRecipeAsync_ReturnsNoContent()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetAllRecipeAsync_ReturnsOK()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRandomRecipeAsync_ReturnsOK()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeByIdAsync_IdDoesNotExist_ReturnsOK()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeByIdAsync_ReturnsOK()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithCuisinesAsync_NoRecipesFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithCuisinesAsync_CuisineDoesNotExists_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithCuisinesAsync_NoCuisinesGiven_ReturnsOk_WithAllRecipes()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithIngredientsAsync_NoRecipesFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithIngredientsAsync_IngredientDoesNotExists_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithIngredientsAsync_NoIngredientGiven_ReturnsOk_WithAllRecipes()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithRestrictionAsync_NoRecipesFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithRestrictionAsync_RestrictionDoesNotExists_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetRecipeWithRestrictionAsync_NoRestrictionGiven_ReturnsOk_WithAllRecipes()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateRecipeAsync_IdNotFound_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateRecipeAsync_RecipeWithSameName_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateRecipeAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }
    }
}
