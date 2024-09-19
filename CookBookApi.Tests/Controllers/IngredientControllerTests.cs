using AutoMapper;
using CookBookApi.Controllers;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Http.HttpResults;
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
            _controller = new IngredientsController(_ingredientRepository.Object, _mapper);
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

            _recipeRepository.Setup(r => r.)

            var result = await _controller.DeleteIngredientAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteIngredientAsync_ReturnsNoContent()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetAllIngredientsAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetIngredientsByIdAsync_InvalidId_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task GetIngredientsByIdAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateIngredientAsync_IngredientNotExists_ReturnsNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateIngredientAsync_ExistingName_ReturnsBadRequest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public async Task UpdateIngredientAsync_ReturnsOk()
        {
            throw new NotImplementedException();
        }
    }
}
