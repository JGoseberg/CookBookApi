using AutoMapper;
using CookBookApi.Controllers;
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
        private Mock<IRecipeRepository> _recipeRepository;

        [SetUp]
        public void Setup()
        {
            _ingredientRepository = new Mock<IIngredientRepository>();
            _recipeRepository = new Mock<IRecipeRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _controller = new IngredientsController(_ingredientRepository.Object, _recipeRepository.Object, _mapper);
        }

        [Test]
        public async Task AddIngredientAsync_NameisEmpty_ReturnsBadRequest() 
        {
            var ingredientDto = new IngredientDto { Name = string.Empty };

            var result = await _controller.AddIngredientAsync(ingredientDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientAsync_NameExists_ReturnsBadRequest()
        {
            var ingredientDto = new IngredientDto { Name = "Double" };

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameName(ingredientDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddIngredientAsync(ingredientDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientAsync_ReturnsCreated()
        {
            var ingredientDto = new IngredientDto { Name = "Foo" };

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameName(ingredientDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddIngredientAsync(ingredientDto);

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

            _recipeRepository.Setup(r => r.AnyRecipeWithIngredientAsync(id))
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

            _recipeRepository.Setup(r => r.AnyRecipeWithIngredientAsync(id))
                .ReturnsAsync(false);

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

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameName(ingredientDto.Name))
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

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameName(ingredientDto.Name))
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

            _ingredientRepository.Setup(i => i.AnyIngredientWithSameName(ingredientDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateIngredientAsync(id, ingredientDto);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
