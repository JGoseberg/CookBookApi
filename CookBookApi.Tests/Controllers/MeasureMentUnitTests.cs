using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs;
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
        public void AddMeasurementUnitTest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void RemoveMeasurementUnitTest() { throw new NotImplementedException(); }

        [Test]
        public async Task GetMeasurementUnitTest()
        {
            var units = new List<MeasurementUnitDto>
            {
                new MeasurementUnitDto { Id = 1, Name = "gram", Abbreviation = "g"},
                new MeasurementUnitDto { Id = 2, Name = "kilogram", Abbreviation = "kg"},
            };

            _measurementUnitRepositoryMock.Setup(r => r.GetAllMeasurementunitsAsync()).ReturnsAsync(units);
                        
            var  result = await _controller.GetAllMeasurementUnitsAsync();

            Assert.IsInstanceOf<ActionResult<IEnumerable<MeasurementUnitDto>>>(result);

            var returnedUnits = result.Value as List<MeasurementUnitDto>;
            Assert.AreEqual(2, returnedUnits.Count);
        }

        [Test]
        public void GetMeasurementUnitsTests() { throw new NotImplementedException(); }

        [Test]
        public void UpdateMeasurementUnitTest() { throw new NotImplementedException(); }
    }
}
