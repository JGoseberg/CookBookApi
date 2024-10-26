using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class CuisinesControllerTests
    {
        private CuisinesController _controller;
        private Mock<ICuisineRepository> _cuisineRepositoryMock;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _cuisineRepositoryMock = new Mock<ICuisineRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _controller = new CuisinesController(_cuisineRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
        }

        [Test]
        public async Task AddCuisineReturnsOk()
        {
            var cuisineDto = new CuisineDto { Name = "Test" };

            _cuisineRepositoryMock.Setup(x => x.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddCuisineAsync(cuisineDto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task AddCuisineAsync_NameIsEmpty_ReturnsBadRequest()
        {
            var cuisineDto = new CuisineDto { Name= "" };

            var result = await _controller.AddCuisineAsync(cuisineDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddCuisineAsync_NameIsNull_ReturnsBadRequest()
        {
            var result = await _controller.AddCuisineAsync(new CuisineDto());

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddCuisine_NameExists_ReturnsBadRequest()
        {
            var cuisineDto = new CuisineDto { Name = "Test" };

            _cuisineRepositoryMock.Setup(x => x.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddCuisineAsync(cuisineDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteCuisine_ReturnsNoContent()
        {
            var id = 204;

            var cuisineDto = new CuisineDto
            {
                Name = "Test",
            };

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync(cuisineDto);

            _recipeRepositoryMock.Setup(r => r.AnyRecipesWithCuisineAsync(id))
                .ReturnsAsync(false);

            var result = await _controller.DeleteCuisineAsync(id);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteCuisine_NotFound_ReturnsNotFound()
        {
            var id = 999;

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync((CuisineDto?)null);

            var result = await _controller.DeleteCuisineAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteCuisine_hasRelatedRecipes_ReturnsBadRequest()
        {
            var id = 1;

            var cuisineDto = new CuisineDto
            {
                Name = "Test",
            };

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync(cuisineDto);

            _recipeRepositoryMock.Setup(r => r.AnyRecipesWithCuisineAsync(id))
                .ReturnsAsync(true);

            var result = await _controller.DeleteCuisineAsync(id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetCuisines_ReturnsCuisines()
        {
            var cuisines = new List<CuisineDto>
            {
                new CuisineDto { Name = "Test" },
                new CuisineDto { Name = "Test2" }
            };

            _cuisineRepositoryMock.Setup(c => c.GetAllCuisinesAsync())
                .ReturnsAsync(cuisines);

            var result = await _controller.GetCuisinesAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetCuisineById_ReturnsOk()
        {
            var id = 1;

            var cuisine = new CuisineDto { Name = "Test" };

            _cuisineRepositoryMock.Setup(x => x.GetCuisineByIdAsync(id))
                .ReturnsAsync(cuisine);

            var result = await _controller.GetCuisineByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetCuisineById_IdNotExists_ReturnsNotFound()
        {
            var id = 404;

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync((CuisineDto?)null);

            var result = await _controller.GetCuisineByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateCuisineReturnsOk()
        {
            var id = 1;

            var cuisineDto = new CuisineDto { Name="Test" };

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync(cuisineDto);

            _cuisineRepositoryMock.Setup(c => c.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateCuisineAsync(id, cuisineDto);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateCuisineAsync_IdNotFound_ReturnsNotFound()
        {
            var id = 1;

            var cuisineDto = new CuisineDto { Name = "Test" };

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync((CuisineDto?)null);

            _cuisineRepositoryMock.Setup(c => c.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateCuisineAsync(id, cuisineDto);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdateCuisineAsync_CuisineWithSameName_ReturnsBadRequest()
        {
            var id = 1;

            var cuisineDto = new CuisineDto { Name = "Test" };

            _cuisineRepositoryMock.Setup(c => c.GetCuisineByIdAsync(id))
                .ReturnsAsync(cuisineDto);

            _cuisineRepositoryMock.Setup(c => c.AnyCuisineWithSameNameAsync(cuisineDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.UpdateCuisineAsync(id, cuisineDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
