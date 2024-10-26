using AutoMapper;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CookBookApi.Controllers;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class RecipesControllerTests
    {
        private Mock<ICuisineRepository> _cuisineRepositoryMock;
        private Mock<IIngredientRepository> _ingredientRepositoryMock;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private RecipesController _controller;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _cuisineRepositoryMock = new Mock<ICuisineRepository>();
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _controller = new RecipesController(_cuisineRepositoryMock.Object, _ingredientRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
        }

        [Test]
        public async Task AddRecipeAsync_NameIsEmpty_ReturnsBadRequest()
        {
            var recipeDto = new AddRecipeDto { Name = string.Empty };

            var result = await _controller.AddRecipeAsync(recipeDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeAsync_InstructionIsEmpty_ReturnsBadRequest()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = string.Empty
            };

            var result = await _controller.AddRecipeAsync(recipeDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeAsync_RecipeWithExactNameExists_ReturnsBadRequest()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
            };

            _recipeRepositoryMock.Setup(r => r.AnyRecipesWithSameNameAsync(recipeDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddRecipeAsync(recipeDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeAsync_SubRecipesAndParentRecipesNull_ReturnsOk()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
            };

            _recipeRepositoryMock.Setup(r => r.AnyRecipesWithSameNameAsync(recipeDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddRecipeAsync(recipeDto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task AddRecipeAsync_SubRecipesNotNull_AddSubrecipes()
        {
            var subRecipes = new List<RecipeDto>
            {
                new RecipeDto {Name = "Foo", Instruction = "Bar"},
                new RecipeDto {Name = "Bar", Instruction = "Foo"},
            };

            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
                Subrecipes = subRecipes
            };

            _recipeRepositoryMock.Setup(r => r.AnyRecipesWithSameNameAsync(recipeDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddRecipeAsync(recipeDto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());

            var createdResult = result as CreatedResult;

            Assert.That(createdResult.Value, Is.Not.Null);

            var recipe = createdResult.Value as Recipe;

            Assert.That(recipe.Subrecipes.Count.Equals(subRecipes.Count));
        }

        [Test]
        public async Task AddRecipeAsync_ParentRecipesNotNull_AddParentRecipes()
        {
            var parentRecipes = new List<RecipeDto>
            {
                new RecipeDto {Name = "Foo", Instruction = "Bar"},
                new RecipeDto {Name = "Bar", Instruction = "Foo"},
            };

            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
                ParentRecipes = parentRecipes
            };

            _recipeRepositoryMock.Setup(r => r.AnyRecipesWithSameNameAsync(recipeDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddRecipeAsync(recipeDto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());

            var createdResult = result as CreatedResult;

            Assert.That(createdResult.Value, Is.Not.Null);

            var recipe = createdResult.Value as Recipe;

            Assert.That(recipe.ParentRecipes.Count.Equals(parentRecipes.Count));
        }

        [Test]
        public async Task DeleteRecipeAsync_NotFound_ReturnsNotFound()
        {
            var id = 404;

            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(id))
                .ReturnsAsync((RecipeDto?)null);

            var result = await _controller.DeleteRecipeAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteRecipeAsync_ReturnsNoContent()
        {
            var id = 404;

            var recipeDto = new RecipeDto { Name = "Foo", Instruction = "Bar" };

            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(id))
                .ReturnsAsync(recipeDto);

            var result = await _controller.DeleteRecipeAsync(id);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllRecipeAsync_ReturnsOK()
        {
            var recipes = new List<RecipeDto>()
            {
                new RecipeDto { Name ="Foo", Instruction = "Bar" },
                new RecipeDto { Name ="Bar", Instruction ="Foo" },
            };

            _recipeRepositoryMock.Setup(r => r.GetAllRecipesAsync())
                .ReturnsAsync(recipes);

            var result = await _controller.GetAllRecipesAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
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
