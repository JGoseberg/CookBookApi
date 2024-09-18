using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using CookBookApi.Repositories;
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
    public class RestrictionControllerTests
    {
        private RestrictionsController _controller;
        private Mock<IRestrictionRepository> _restrictionRepository;
        private Mock<IIngredientRepository> _ingredientRepository;
        private Mock<IRecipeRepository> _recipeRepository;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _restrictionRepository = new Mock<IRestrictionRepository>();
            _ingredientRepository = new Mock<IIngredientRepository>();
            _recipeRepository = new Mock<IRecipeRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _controller = new RestrictionsController(_restrictionRepository.Object, _ingredientRepository.Object, _recipeRepository.Object, _mapper);
        }

        [Test]
        public async Task AddRestrictionAsync_NameIsEmpty_ReturnsBadRequest()
        {
            var restrictionDto = new RestrictionDto { Name = "" };

            var result = await _controller.AddRestrictionAsync(restrictionDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRestrictionAsync_NameExists_ReturnsBadRequest()
        {
            var restrictionDto = new RestrictionDto { Name = "Double" };

            _restrictionRepository.Setup(r => r.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddRestrictionAsync(restrictionDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRestrictionAsync_ReturnsOK()
        {
            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddRestrictionAsync(restrictionDto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }


        [Test]
        public async Task DeleteRestriction_RestrictionDoesNotExists_ReturnsNotFound()
        {
            var id = 404;

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync((RestrictionDto?)null);

            var result = await _controller.DeleteRestrictionAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteRestriction_RelatedToIngredient_ReturnsBadRequest()
        {
            var id = 400;

            var restrictionDto = new RestrictionDto {  Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _ingredientRepository.Setup(i => i.AnyIngredientsWithRestrictionAsync(id))
                .ReturnsAsync(true);

            var result = await _controller.DeleteRestrictionAsync(id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteRestriction_RelatedToRecipe_ReturnsBadRequest()
        {
            var id = 400;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _ingredientRepository.Setup(i => i.AnyIngredientsWithRestrictionAsync(id))
                .ReturnsAsync(false);

            _recipeRepository.Setup(rc => rc.AnyRecipesWithRestrictionAsync(id))
                .ReturnsAsync(true);

            var result = await _controller.DeleteRestrictionAsync(id);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task DeleteRestriction_ReturnsNoContent()
        {

            var id = 200;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _ingredientRepository.Setup(i => i.AnyIngredientsWithRestrictionAsync(id))
                .ReturnsAsync(false);

            _recipeRepository.Setup(rc => rc.AnyRecipesWithRestrictionAsync(id))
                .ReturnsAsync(false);

            var result = await _controller.DeleteRestrictionAsync(id);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllRestrictions_ReturnsOk()
        {
            var restrictions = new RestrictionDto[]
            {
                new RestrictionDto { Name = "Foo"},
                new RestrictionDto{ Name = "Bar"}
            };

            _restrictionRepository.Setup(r => r.GetAllRestrictionsAsync())
                .ReturnsAsync(restrictions);

            var result = await _controller.GetAllRestrictionsAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetRestrictionById_InvalidInt_ReturnsNotFound()
        {
            var id = 404;

            var restriction = new RestrictionDto { Name= "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync((RestrictionDto?)null);

            var result = await _controller.GetRestrictionByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetRestrictionById_ValidInt_ReturnsOk()
        {
            var id = 200;

            var restriction = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restriction);

            var result = await _controller.GetRestrictionByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateRestrictionsAsync_NotExistingRestriction_ReturnsNotFound()
        {
            var id = 404;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync((RestrictionDto?)null);

            var result = await _controller.UpdateRestrictionAsync(id, restrictionDto);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdateRestrictionsAsync_ExistingName_ReturnsBadRequest()
        {
            var id = 400;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _restrictionRepository.Setup(r => r.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.UpdateRestrictionAsync(id, restrictionDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateRestrictionsAsync_ReturnsOk()
        {
            var id = 200;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _restrictionRepository.Setup(r => r.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.UpdateRestrictionAsync(id, restrictionDto);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }
    }
}
