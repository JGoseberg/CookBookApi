using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class MeasureMentUnitControllerTests
    {
        private MeasurementUnitController _controller;
        private Mock<IMeasurementUnitRepository> _measurementUnitRepository;
        private Mock<IRecipeIngredientRepository> _recipeIngredientRepository;
        private IMapper _mapper;
        private Mock<IRecipeRepository> _recipeRepository;

        [SetUp]
        public void Setup()
        {
            _measurementUnitRepository = new Mock<IMeasurementUnitRepository>();
            _recipeIngredientRepository = new Mock<IRecipeIngredientRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _recipeRepository = new Mock<IRecipeRepository>();
            _controller = new MeasurementUnitController(_measurementUnitRepository.Object, _recipeIngredientRepository.Object, _mapper, _recipeRepository.Object);
        }

        [Test]
        public async Task AddMeasurementUnit_EmptyName_ReturnsBadRequest()
        {
            var addMeasurementUnitDto = new AddMeasurementUnitDto { Name = "", Abbreviation = "bar" };

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);
            
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddMeasurementUnit_EmptyAbbreviation_ReturnsBadRequest()
        {
            var addMeasurementUnitDto = new AddMeasurementUnitDto { Name = "foo", Abbreviation = "" };

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddMeasurementUnit_NameExists_ReturnsBadRequest()
        {
            var addMeasurementUnitDto = new AddMeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.AnyMeasurementUnitWithSameNameAsync(addMeasurementUnitDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddMeasurementUnitTest_ReturnsOk()
        {
            var addMeasurementUnitDto = new AddMeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.AnyMeasurementUnitWithSameNameAsync(addMeasurementUnitDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);

            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [Test]
        public async Task DeleteMeasurementUnit_MeasurementUnitDoesNotExists_ReturnsNotFound()
        {
            var id = 404;

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync((MeasurementUnitDto?)null);

            var result = await _controller.DeleteMeasurementUnitAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteMeasurementUnit_RelatedToRecipe_ReturnsBadRequest()
        {
            var id = 404;

            var measurementUnitDto = new MeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            _recipeIngredientRepository.Setup(ri => ri.AnyRecipesWithMeasurementUnitAsync(id))
                .ReturnsAsync(true);

            var result = await _controller.DeleteMeasurementUnitAsync(id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteMeasurementUnit_ReturnsNoContent()
        {
            var id = 404;

            var measurementUnitDto = new MeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            _recipeIngredientRepository.Setup(ri => ri.AnyRecipesWithMeasurementUnitAsync(id))
                .ReturnsAsync(false);

            var result = await _controller.DeleteMeasurementUnitAsync(id);

            Assert.That(result, Is.InstanceOf<NoContentResult>());

        }

        [Test]
        public async Task GetAllMeasurements_ReturnsOk()
        {
            IEnumerable<MeasurementUnitDto> measurementUnits = new MeasurementUnitDto[]
            {
                new MeasurementUnitDto { Name =  "foo", Abbreviation = "bar" },
                new MeasurementUnitDto { Name = "bar", Abbreviation = "foo"}
            };

            _measurementUnitRepository.Setup(m => m.GetAllMeasurementunitsAsync())
                .ReturnsAsync(measurementUnits);

            var result = await _controller.GetAllMeasurementUnitsAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetMeasurementUnitById_ValidInt_ReturnsOk()
        {
            var id = 1;

            MeasurementUnitDto measurementUnitDto = new()
            { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            var result = await _controller.GetMeasurementUnitByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetMeasurementUnitById_InvalidInt_ReturnsNotFound()
        {
            var id = 404;

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync((MeasurementUnitDto?)null);

            var result = await _controller.GetMeasurementUnitByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateMeasurementUnitTest_NotExistingMeasurementUnit_ReturnsNotFound()
        {
            var id = 404;

            MeasurementUnitDto measurementUnitDto = new()
            { Name = "foo", Abbreviation= "bar" };

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync((MeasurementUnitDto?)null);

            var result = await _controller.UpdateMeasurementUnitAsync(id, measurementUnitDto);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdateMeasurementUnitTest_ExistingName_ReturnsBadRequest()
        {
            var id = 400;

            MeasurementUnitDto measurementUnitDto = new()
            { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            _measurementUnitRepository.Setup(m => m.AnyMeasurementUnitWithSameNameAsync(measurementUnitDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.UpdateMeasurementUnitAsync(id, measurementUnitDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateMeasurementUnitTest_ReturnsOk()
        {
            var id = 200;

            MeasurementUnitDto measurementUnitDto = new()
            { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepository.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            _measurementUnitRepository.Setup(m => m.AnyMeasurementUnitWithSameNameAsync(measurementUnitDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateMeasurementUnitAsync(id, measurementUnitDto);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        }
    }
}
