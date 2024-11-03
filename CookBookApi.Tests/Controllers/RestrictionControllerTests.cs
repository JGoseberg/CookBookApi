using CookBookApi.Controllers;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class RestrictionControllerTests
    {
        private RestrictionsController _controller;
        private Mock<IRestrictionRepository> _restrictionRepository;
        private Mock<IIngredientRestrictionRepository> _ingredientRestrictionRepository;
        private Mock<IRecipeRestrictionRepository> _recipeRestrictionRepository;

        [SetUp]
        public void Setup()
        {
            _restrictionRepository = new Mock<IRestrictionRepository>();
            _ingredientRestrictionRepository = new Mock<IIngredientRestrictionRepository>();
            _recipeRestrictionRepository = new Mock<IRecipeRestrictionRepository>();
            _controller = new RestrictionsController(_ingredientRestrictionRepository.Object, _recipeRestrictionRepository.Object, _restrictionRepository.Object);
        }

        [Test]
        public async Task AddRestrictionAsync_NameIsEmpty_ShouldReturnBadRequest()
        {
            var restrictionDto = new RestrictionDto { Name = "" };

            var result = await _controller.AddRestrictionAsync(restrictionDto);

            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(400));
                Assert.That(resultValue.Value, Is.EqualTo($"Name {restrictionDto.Name} cannot be empty."));
            });
        }
        
        [Test]
        public async Task AddRestrictionAsync_NameIsNull_ShouldReturnBadRequest()
        {
            var restrictionDto = new RestrictionDto { Name = string.Empty };

            var result = await _controller.AddRestrictionAsync(restrictionDto);

            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(400));
                Assert.That(resultValue.Value, Is.EqualTo($"Name {restrictionDto.Name} cannot be empty."));
            });
        }
        
        [Test]
        public async Task AddRestrictionAsync_NameExists_ShouldReturnBadRequest()
        {
            var restrictionDto = new RestrictionDto { Name = "Double" };

            _restrictionRepository.Setup(r => r.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                .ReturnsAsync(true);

            var result = await _controller.AddRestrictionAsync(restrictionDto);
            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(400));
                Assert.That(resultValue.Value, Is.EqualTo($"A Restriction with the Name {restrictionDto.Name} already exists"));
            });
        }

        [Test]
        public async Task AddRestrictionAsync_ShouldReturnOK()
        {
            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.AnyRestrictionWithSameNameAsync(restrictionDto.Name))
                .ReturnsAsync(false);

            var result = await _controller.AddRestrictionAsync(restrictionDto);
            var resultValue = (CreatedResult)result;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<CreatedResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(201));
                Assert.That(resultValue.Value, Is.InstanceOf<Restriction>());
            });
        }


        [Test]
        public async Task DeleteRestrictionAsync_RestrictionDoesNotExists_ShouldReturnNotFound()
        {
            var id = 404;

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync((RestrictionDto?)null);

            var result = await _controller.DeleteRestrictionAsync(id);
            var resultValue = (NotFoundObjectResult)result;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(404));
                Assert.That(resultValue.Value, Is.EqualTo($"Restriction with Id {id} was not found."));
            });
        }

        [Test]
        public async Task DeleteRestrictionAsync_RelatedToIngredient_ShouldReturnBadRequest()
        {
            var id = 400;

            var restrictionDto = new RestrictionDto {  Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _ingredientRestrictionRepository.Setup(i => i.AnyIngredientWithRestrictionAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _controller.DeleteRestrictionAsync(id);
            var resultValue = (BadRequestObjectResult)result;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(400));
                Assert.That(resultValue.Value, Is.EqualTo($"restriction with Id {id} has existing Relations to Ingredients, cannot delete."));
            });
        }

        [Test]
        public async Task DeleteRestrictionAsync_RelatedToRecipe_ShouldReturnBadRequest()
        {
            var id = 400;

            var restrictionDto = new RestrictionDto {  Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _ingredientRestrictionRepository.Setup(i => i.AnyIngredientWithRestrictionAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            
            _recipeRestrictionRepository.Setup(rr => rr.AnyRecipeWithRestrictionAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            
            var result = await _controller.DeleteRestrictionAsync(id);
            var resultValue = (BadRequestObjectResult)result;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(400));
                Assert.That(resultValue.Value, Is.EqualTo($"restriction with Id {id} has existing Relations to Recipes, cannot delete."));
            });
        }
        
        [Test]
        public async Task DeleteRestrictionAsync_ShouldReturnNoContent()
        {

            var id = 200;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restrictionDto);

            _ingredientRestrictionRepository.Setup(i => i.AnyIngredientWithRestrictionAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            
            _recipeRestrictionRepository.Setup(rr => rr.AnyRecipeWithRestrictionAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _controller.DeleteRestrictionAsync(id);
            var resultValue = (NoContentResult)result;

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                Assert.That(resultValue.StatusCode, Is.EqualTo(204));
            });
        }

        [Test]
        public async Task GetAllRestrictionsAsync_ShouldReturnOk()
        {
            var restrictions = new[]
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
        public async Task GetRestrictionByIdAsync_InvalidInt_ShouldReturnNotFound()
        {
            var id = 404;

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync((RestrictionDto?)null);

            var result = await _controller.GetRestrictionByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetRestrictionByIdAsync_ValidInt_ShouldReturnOk()
        {
            var id = 200;

            var restriction = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync(restriction);

            var result = await _controller.GetRestrictionByIdAsync(id);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task UpdateRestrictionsAsync_NotExistingRestriction_ShouldReturnNotFound()
        {
            var id = 404;

            var restrictionDto = new RestrictionDto { Name = "Foo" };

            _restrictionRepository.Setup(r => r.GetRestrictionByIdAsync(id))
                .ReturnsAsync((RestrictionDto?)null);

            var result = await _controller.UpdateRestrictionAsync(id, restrictionDto);

            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }
        
        [Test]
        public async Task UpdateRestrictionsAsync_ExistingName_ShouldReturnBadRequest()
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
        public async Task UpdateRestrictionsAsync_ShouldReturnOk()
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
