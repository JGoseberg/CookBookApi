using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class MeasureMentUnitTests
    {
        private MeasurementUnitController _controller;
        private Mock<IMeasurementUnitRepository> _measurementUnitRepositoryMock;
        private Mock<IRecipeIngredientRepository> _recipeIngredientRepositoryMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _measurementUnitRepositoryMock = new Mock<IMeasurementUnitRepository>();
            _recipeIngredientRepositoryMock = new Mock<IRecipeIngredientRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _controller = new MeasurementUnitController(_measurementUnitRepositoryMock.Object, _recipeIngredientRepositoryMock.Object, _mapper);
        }

        [Test]
        public async Task AddMeasurementUnit_EmptyName_ReturnsBadRequest()
        {
            var addMeasurementUnitDto = new AddMeasurementUnitDto { Name = "", Abbreviation = "bar" };

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);

            // more unique Asserts
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

            _measurementUnitRepositoryMock.Setup(m => m.AnyMeasurementUnitWithSameNameAsync(addMeasurementUnitDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddMeasurementUnitTest_ReturnsOk()
        {
            var addMeasurementUnitDto = new AddMeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepositoryMock.Setup(m => m.AnyMeasurementUnitWithSameNameAsync(addMeasurementUnitDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddMeasurementUnitAsync(addMeasurementUnitDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public async Task DeleteMeasurementUnit_MeasurementUnitDoesNotExists_ReturnsNotFound()
        {
            var id = 404;

            _measurementUnitRepositoryMock.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync((MeasurementUnitDto?)null);

            var result = await _controller.DeleteMeasurementUnitAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteMeasurementUnit_RelatedToRecipe_ReturnsBadRequest()
        {
            var id = 404;

            var measurementUnitDto = new MeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepositoryMock.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            _recipeIngredientRepositoryMock.Setup(ri => ri.AnyRecipesWithMeasurementUnitAsync(id))
                .ReturnsAsync(true);

            var result = await _controller.DeleteMeasurementUnitAsync(id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteMeasurementUnit_ReturnsNoContent()
        {
            var id = 404;

            var measurementUnitDto = new MeasurementUnitDto { Name = "foo", Abbreviation = "bar" };

            _measurementUnitRepositoryMock.Setup(m => m.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync(measurementUnitDto);

            _recipeIngredientRepositoryMock.Setup(ri => ri.AnyRecipesWithMeasurementUnitAsync(id))
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

            _measurementUnitRepositoryMock.Setup(m => m.GetAllMeasurementunitsAsync())
                .ReturnsAsync(measurementUnits);

            var result = await _controller.GetAllMeasurementUnitsAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [TestCase(1)]
        public async Task GetMeasurementUnitById_ValidInt_ReturnsOk(int id)
        {
            throw new NotImplementedException();
        }

        [TestCase(99)]
        public async Task GetMeasurementUnitById_InValidInt_ReturnsNotFound(int id)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void UpdateMeasurementUnitTest_NotExistingMeasurementUnit_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void UpdateMeasurementUnitTest_ExistingName_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void UpdateMeasurementUnitTest_ReturnsOk()
        {
            throw new NotImplementedException();
        }
    }
}
