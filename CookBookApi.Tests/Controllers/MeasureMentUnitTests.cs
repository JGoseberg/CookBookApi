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
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _measurementUnitRepositoryMock = new Mock<IMeasurementUnitRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _controller = new MeasurementUnitController(_measurementUnitRepositoryMock.Object, _mapper);
        }

        [Test]
        public async Task AddMeasurementUnitTest_ReturnsOk()
        {
            var unitDto = new AddMeasurementUnitDto
            {
                Name = "Test",
                Abbreviation = "t",
            };

            var unit = new MeasurementUnit
            {
                Id = 1,
                Name = unitDto.Name,
                Abbreviation = unitDto.Abbreviation
            };

            _measurementUnitRepositoryMock
                .Setup(r => r.AnyMeasurementUnitWithSameNameAsync(unitDto.Name))
                .ReturnsAsync(false);

            _measurementUnitRepositoryMock
                .Setup(r => r.AddMeasurementUnitAsync(It.IsAny<MeasurementUnit>()))
                .ReturnsAsync(unit);

            var result = await _controller.AddMeasurementUnitAsync(unitDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public void AddExistingMeasurementUnit_ReturnsBadRequest()
        { 
            throw new NotImplementedException();
        }

        [Test]
        public void AddIncompleteMeasurementUnit_ReturnsBadRequest() { throw new NotImplementedException(); }

        [Test]
        public void RemoveMeasurementUnitTest()
        {
            throw new NotImplementedException();
        }

        [TestCase(1)]
        public async Task GetMeasurementUnitById_ValidInt_ReturnsOk(int id)
        {
            var unit = new MeasurementUnitDto
            {
                Id = 1,
                Name = "gram",
                Abbreviation = "g"
            };

            _measurementUnitRepositoryMock.Setup(r => r.GetMeasurementUnitByIdAsync(id)).ReturnsAsync(unit);

            var result = await _controller.GetMeasurementUnitByIdAsync(id);

            var okResult = result.Result as OkObjectResult;

            Assert.That(unit, Is.EqualTo(okResult.Value));

            Assert.IsNotNull(result);
        }

        [TestCase(99)]
        public async Task GetMeasurementUnitById_InValidInt_Returns404(int id)
        {
            _measurementUnitRepositoryMock
                .Setup(r => r.GetMeasurementUnitByIdAsync(id))
                .ReturnsAsync((MeasurementUnitDto)null);

            var result = await _controller.GetMeasurementUnitByIdAsync(id);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetMeasurementUnitsTests()
        {
            var units = new List<MeasurementUnitDto>
            {
                new MeasurementUnitDto { Id = 1, Name = "gram", Abbreviation = "g"},
                new MeasurementUnitDto { Id = 2, Name = "kilogram", Abbreviation = "kg"},
            };

            _measurementUnitRepositoryMock.Setup(r => r.GetAllMeasurementunitsAsync()).ReturnsAsync(units);

            var result = await _controller.GetAllMeasurementUnitsAsync();

            Assert.IsInstanceOf<ActionResult<IEnumerable<MeasurementUnitDto>>>(result);

            var returnedUnits = result.Value as List<MeasurementUnitDto>;
            Assert.AreEqual(2, returnedUnits.Count);
        }

        [Test]
        public void UpdateMeasurementUnitTest()
        {
            throw new NotImplementedException();
        }
    }
}
