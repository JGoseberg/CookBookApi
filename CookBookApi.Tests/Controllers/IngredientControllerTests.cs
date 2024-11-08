using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    class IngredientControllerTests
    {
        private IngredientsController _controller;
        private IMapper _mapper;
        private Mock<IIngredientRepository> _ingredientRepository;
        private Mock<IRecipeIngredientRepository> _recipeIngredientRepository;
        private Mock<ICuisineRepository> _cuisineRepository;

        [SetUp]
        public void Setup()
        {
            _cuisineRepository = new Mock<ICuisineRepository>();
            _ingredientRepository = new Mock<IIngredientRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _recipeIngredientRepository = new Mock<IRecipeIngredientRepository>();
            _controller = new IngredientsController(_cuisineRepository.Object, _ingredientRepository.Object, _recipeIngredientRepository.Object, _mapper);
        }

        [Test]
        public async Task AddIngredientAsync_NameIsEmpty_ReturnsBadRequest() 
        {
            var ingredientDto = new IngredientDto
            {
                Name = string.Empty,
                Cuisine = new CuisineDto { Id = 1, Name = string.Empty }
            };
            
            var result = await _controller.AddIngredientAsync(ingredientDto.Name, ingredientDto.Cuisine.Id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientAsync_NameExists_ReturnsBadRequest()
        {
            var ingredientDto = new IngredientDto
            {
                Name = "duplicate",
                Cuisine = new CuisineDto { Id = 1, Name = string.Empty}
            };

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameNameAsync(ingredientDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddIngredientAsync(ingredientDto.Name, ingredientDto.Cuisine.Id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientAsync_ReturnsCreated()
        {
            var ingredientDto = new IngredientDto { Name = "Foo", Cuisine = new CuisineDto { Id = 1 ,Name = string.Empty } };

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameNameAsync(ingredientDto.Name))
                .ReturnsAsync(false);
            
            _cuisineRepository.Setup(c => c.GetCuisineByIdAsync(ingredientDto.Cuisine.Id))
                .ReturnsAsync(ingredientDto.Cuisine);

            var result = await _controller.AddIngredientAsync(ingredientDto.Name, ingredientDto.Cuisine.Id);

            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task DeleteIngredientAsync_IngredientDoesNotExist_ReturnsNotFound()
        {
            var id = 404;

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync((IngredientDto?)null);

            var result = await _controller.DeleteIngredientAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteIngredientAsync_RelatedToRecipe_ReturnsBadRequest()
        {
            var id = 400;

            var ingredientDto = new IngredientDto { Name= "Foo" };

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync(ingredientDto);

            _recipeIngredientRepository.Setup(ri => ri.AnyRecipesWithIngredientAsync(id))
                .ReturnsAsync(true);
            
            var result = await _controller.DeleteIngredientAsync(id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteIngredientAsync_ReturnsNoContent()
        {
            var id = 204;

            var ingredientDto = new IngredientDto { Name = "Foo" };

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync(ingredientDto);

            var result = await _controller.DeleteIngredientAsync(id);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllIngredientsAsync_ReturnsOk()
        {
            var ingredients = new List<IngredientDto>
            {
                new IngredientDto { Name = "Foo"},
                new IngredientDto { Name = "Bar"}
            };

            _ingredientRepository.Setup(i => i.GetAllIngredientsAsync())
                .ReturnsAsync(ingredients);

            var result = await _controller.GetAllIngredientsAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetIngredientsByIdAsync_InvalidId_ReturnsNotFound()
        {
            var id = 404;
            
            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync((IngredientDto?)null);

            var result = await _controller.GetIngredientByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetIngredientsByIdAsync_ReturnsOk()
        {
            var id = 404;

            var ingredientDto = new IngredientDto { Name = "Foo" };

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync(ingredientDto);

            var result = await _controller.GetIngredientByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateIngredientAsync_IngredientNotExists_ReturnsNotFound()
        {
            var id = 404;

            var ingredientDto = new IngredientDto { Name = "Foo" };

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync((IngredientDto?)null);

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameNameAsync(ingredientDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateIngredientAsync(id, ingredientDto);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdateIngredientAsync_ExistingName_ReturnsBadRequest()
        {
            var id = 404;

            var ingredientDto = new IngredientDto { Name = "Foo" };

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync(ingredientDto);

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameNameAsync(ingredientDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.UpdateIngredientAsync(id, ingredientDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateIngredientAsync_ReturnsOk()
        {
            var id = 404;

            var ingredientDto = new IngredientDto { Name = "Foo" };

            _ingredientRepository.Setup(i => i.GetIngredientByIdAsync(id))
                .ReturnsAsync(ingredientDto);

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameNameAsync(ingredientDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateIngredientAsync(id, ingredientDto);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
