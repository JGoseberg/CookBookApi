using AutoMapper;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Mappings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CookBookApi.Controllers;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.Interfaces;

namespace CookBookApi.Tests.Controllers
{
    [TestFixture]
    public class RecipesControllerTests
    {
        private Mock<ICuisineRepository> _cuisineRepositoryMock;
        private Mock<IIngredientRepository> _ingredientRepositoryMock;
        private IMapper _mapper;
        private RecipesController _controller;
        private Mock<IRecipeIngredientRepository> _recipeIngredientRepositoryMock;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private Mock<IRecipeRestrictionRepository> _recipeRestrictionRepositoryMock;
        private Mock<IRecipeService> _recipeServiceMock;
        private Mock<IRestrictionRepository> _restrictionRepositoryMock;
        
        [SetUp]
        public void Setup()
        {
            _cuisineRepositoryMock = new Mock<ICuisineRepository>();
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _mapper = MapperTestConfig.InitializeAutoMapper();
            _recipeIngredientRepositoryMock = new Mock<IRecipeIngredientRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _recipeRestrictionRepositoryMock = new Mock<IRecipeRestrictionRepository>();
            _recipeServiceMock = new Mock<IRecipeService>();
            _restrictionRepositoryMock = new Mock<IRestrictionRepository>();
            _controller = new RecipesController
                (
                    _cuisineRepositoryMock.Object,
                    _ingredientRepositoryMock.Object,
                    _mapper,
                    _recipeIngredientRepositoryMock.Object,
                    _recipeRepositoryMock.Object,
                    _recipeRestrictionRepositoryMock.Object,
                    _recipeServiceMock.Object,
                    _restrictionRepositoryMock.Object
                );
        }

        [Test]
        public async Task AddRecipeAsync_NameIsEmpty_ShouldReturnBadRequest()
        {
            var recipeDto = new AddRecipeDto { Name = string.Empty };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.Value, Is.EqualTo("Name Cannot be empty."));
            });
        }
        
        [Test]
        public async Task AddRecipeAsync_NameIsNull_ShouldReturnBadRequest()
        {
            var recipeDto = new AddRecipeDto { Name = null! };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.Value, Is.EqualTo("Name Cannot be empty."));
            });
        }

        [Test]
        public async Task AddRecipeAsync_InstructionIsEmpty_ShouldReturnBadRequest()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = string.Empty
            };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.Value, Is.EqualTo("Name Cannot be empty."));
            });
        }
        
        [Test]
        public async Task AddRecipeAsync_InstructionIsNull_ShouldReturnBadRequest()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = string.Empty
            };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (BadRequestObjectResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue.Value, Is.EqualTo("Name Cannot be empty."));
            });
        }

        [Test]
        public async Task AddRecipeAsync_RecipeWithExactNameExists_ShouldReturnCreated()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
                Subrecipes = [],
                ParentRecipes = []
            };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (CreatedResult)result;
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<CreatedResult>());
                Assert.That(resultValue.Value, Is.TypeOf<Recipe>());
            });
        }

        [Test]
        public async Task AddRecipeAsync_SubRecipesAndParentRecipesEmpty_ShouldReturnCreated()
        {
            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
                Subrecipes = [],
                ParentRecipes = [],
            };

            var result = await _controller.AddRecipeAsync(recipeDto);
            
            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task AddRecipeAsync_SubRecipesNotNull_AddsSubRecipes()
        {
            var subRecipes = new List<RecipeDto>
            {
                new RecipeDto {Name = "Foo", Instruction = "Bar"},
                new RecipeDto {Name = "Bar", Instruction = "Foo"},
            };

            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
                Subrecipes = subRecipes,
                ParentRecipes = [],
            };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (CreatedResult)result;
            var recipe = resultValue.Value as Recipe;
            
            Assert.Multiple(() =>
            {
                Assert.That(resultValue.Value, Is.Not.Null);
                Assert.That(recipe!.Subrecipes.Count, Is.EqualTo(subRecipes.Count));
            });
        }

        [Test]
        public async Task AddRecipeAsync_ParentRecipesNotNull_ShouldAddParentRecipes()
        {
            var parentRecipes = new List<RecipeDto>
            {
                new RecipeDto {Name = "Foo", Instruction = "Bar"},
                new RecipeDto {Name = "Bar", Instruction = "Foo"},
            };

            var recipeDto = new AddRecipeDto
            {
                Name = "Foo",
                Instruction = "Bar",
                Subrecipes = [],
                ParentRecipes = parentRecipes,
            };

            var result = await _controller.AddRecipeAsync(recipeDto);
            var resultValue = (CreatedResult)result;
            var recipe = resultValue.Value as Recipe;
            
            Assert.Multiple(() =>
            {
                Assert.That(resultValue.Value, Is.Not.Null);
                Assert.That(recipe!.ParentRecipes.Count, Is.EqualTo(parentRecipes.Count));
            });
        }

        [Test]
        public async Task DeleteRecipeAsync_NotFound_ReturnsNotFound()
        {
            var id = 404;

            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(id))
                .ReturnsAsync((RecipeDto?)null);

            var result = await _controller.DeleteRecipeAsync(id);

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task DeleteRecipeAsync_ReturnsNoContent()
        {
            var id = 404;

            var recipeDto = new RecipeDto { Name = "Foo", Instruction = "Bar" };

            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(id))
                .ReturnsAsync(recipeDto);

            var result = await _controller.DeleteRecipeAsync(id);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GetAllRecipeAsync_ReturnsOK()
        {
            var recipes = new List<RecipeDto>()
            {
                new RecipeDto { Name ="Foo", Instruction = "Bar" },
                new RecipeDto { Name ="Bar", Instruction ="Foo" },
            };

            _recipeRepositoryMock.Setup(r => r.GetAllRecipesAsync())
                .ReturnsAsync(recipes);

            var result = await _controller.GetAllRecipesAsync();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetRandomRecipeAsync_ShouldReturnRandomRecipe()
        {
            var recipeDto = new RecipeDto
            {
                Name = "Foo",
                Description = "Bar",
            };

            _recipeServiceMock.Setup(r => r.GetRandomRecipeAsync())
                .ReturnsAsync(recipeDto);
            
            var result = await _controller.GetRandomRecipeAsync();
            var resultValue = result.Result as OkObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That(resultValue!.Value, Is.InstanceOf<RecipeDto>());
            });
        }

        [Test]
        public async Task GetRecipeByIdAsync_IdDoesNotExist_ShouldReturnNotFound()
        {
            var invalidId = 404;
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(invalidId))
                .ReturnsAsync((RecipeDto?)null);
            
            var result = await _controller.GetRecipeByIdAsync(invalidId);
            
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task GetRecipeByIdAsync_ShouldReturnOK()
        {
            var recipeDto = new RecipeDto
            {
                Id = 1,
                Name = "Foo",
            };
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(recipeDto.Id))
                .ReturnsAsync(recipeDto);
            
            var result = await _controller.GetRecipeByIdAsync(recipeDto.Id);
            
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task GetRecipeWithCuisinesAsync_CuisineDoesNotExists_ReturnsBadRequest()
        {
            var invalidId = 404;
            
            var result = await _controller.GetRecipesWithCuisinesAsync(invalidId);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("Cuisine does not exists!"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithCuisinesAsync_NoRecipesFound_ReturnsNotFound()
        {
            var validCuisine = new CuisineDto
            {
                Name = "Foo",
            };

            _cuisineRepositoryMock.Setup(r => r.GetCuisineByIdAsync(validCuisine.Id))
                .ReturnsAsync(validCuisine);
            
            var result = await _controller.GetRecipesWithCuisinesAsync(validCuisine.Id);
            var resultValue = result.Result as NotFoundObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo($"No recipes with cuisine {validCuisine.Name} found"));
            });
        }

        [Test]
        public async Task GetRecipeWithCuisinesAsync_NoCuisinesGiven_ReturnsOk_WithAllRecipes()
        {
            var validCuisine = new CuisineDto
            {
                Name = "Foo",
            };

            var recipes = new List<RecipeDto>
            {
                new RecipeDto { Name = "Foo", Instruction = "Bar" },
                new RecipeDto { Name = "Bar", Instruction = "Foo" },
            };
            
            _cuisineRepositoryMock.Setup(r => r.GetCuisineByIdAsync(validCuisine.Id))
                .ReturnsAsync(validCuisine);
            
            _recipeRepositoryMock.Setup(r => r.GetRecipesWithSpecificCuisineAsync(validCuisine.Id))
                .ReturnsAsync(recipes);
            
            var result = await _controller.GetRecipesWithCuisinesAsync(validCuisine.Id);
            var resultValue = result.Result as OkObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo(recipes));
            });
        }

        [Test]
        public async Task GetRecipeWithIngredientsAsync_NoIngredientGiven_ShouldReturnBadRequest()
        {
            var ingredientIds = new List<int>();
            
            var result = await _controller.GetRecipesWithIngredientsAsync(ingredientIds);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("Ingredient Ids cannot be empty"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithIngredientsAsync_IngredientDoesNotExists_ShouldReturnBadRequest()
        {
            var ingredientIds = new List<int>{1};
            
            _ingredientRepositoryMock.Setup(r => r.GetIngredientByIdAsync(ingredientIds[0]))
                .ReturnsAsync((IngredientDto?)null);
            
            var result = await _controller.GetRecipesWithIngredientsAsync(ingredientIds);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("Ingredient with Id: 1 not found"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithIngredientsAsync_IngredientsGivenNoRecipesFound_ShouldReturnNotFound()
        {
            var ingredientIds = new List<int> { 1, 2, 3 };
            
            var recipeIds = new List<int> { 4, 5, 6 };

            _ingredientRepositoryMock.Setup(r => r.GetIngredientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new IngredientDto());
            
            _recipeIngredientRepositoryMock.Setup(ri => ri.GetRecipesWithIngredientsAsync(ingredientIds))
                .ReturnsAsync(recipeIds);

            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as RecipeDto);
            
            var result = await _controller.GetRecipesWithIngredientsAsync(ingredientIds);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("No Recipe with this Ingredients found!"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithIngredientsAsync_IngredientsGiven_ShouldReturnRecipes()
        {
            var ingredientIds = new List<int> { 1, 2, 3 };
            
            var recipeIds = new List<int> { 4, 5, 6 };

            var recipeDto = new RecipeDto() { Name = "Foo", Instruction = "Bar" };

            _ingredientRepositoryMock.Setup(r => r.GetIngredientByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new IngredientDto());
            
            _recipeIngredientRepositoryMock.Setup(ri => ri.GetRecipesWithIngredientsAsync(ingredientIds))
                .ReturnsAsync(recipeIds);

            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(recipeDto);
            
            var result = await _controller.GetRecipesWithIngredientsAsync(ingredientIds);
            var resultValue = result.Result as OkObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That(resultValue!.Value, Is.InstanceOf<IEnumerable<RecipeDto>>());
            });
        }

       [Test]
        public async Task GetRecipeWithRestrictionsAsync_NoRestrictionsGiven_ShouldReturnBadRequest()
        {
            var restrictionIds = new List<int>();
            
            var result = await _controller.GetRecipesWithRestrictionsAsync(restrictionIds);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("Restriction Ids cannot be empty"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithRestrictionsAsync_RestrictionDoesNotExists_ShouldReturnBadRequest()
        {
            var restrictionIds = new List<int>{1};
            
            _restrictionRepositoryMock.Setup(r => r.GetRestrictionByIdAsync(restrictionIds[0]))
                .ReturnsAsync((RestrictionDto?)null);
            
            var result = await _controller.GetRecipesWithRestrictionsAsync(restrictionIds);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("Restriction with Id: 1 not found"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithRestrictionsAsync_RestrictionsGivenNoRecipesFound_ShouldReturnNotFound()
        {
            var restrictionIds = new List<int> { 1, 2, 3 };
            
            _restrictionRepositoryMock.Setup(r => r.GetRestrictionByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new RestrictionDto());
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as RecipeDto);
            
            var result = await _controller.GetRecipesWithRestrictionsAsync(restrictionIds);
            var resultValue = result.Result as BadRequestObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo("No Recipe with this Restrictions found!"));
            });
        }
        
        [Test]
        public async Task GetRecipeWithRestrictionsAsync_RestrictionsGiven_ShouldReturnRecipes()
        {
            var restrictionIds = new List<int> { 1, 2, 3 };

            var recipeIds = new List<int> { 4, 5, 6 };

            var recipeDto = new RecipeDto() { Name = "Foo", Instruction = "Bar" };


            _restrictionRepositoryMock.Setup(r => r.GetRestrictionByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new RestrictionDto());
            
            _recipeRestrictionRepositoryMock.Setup(rr => rr.GetRecipeIdsWithRestrictionAsync(restrictionIds))
                .ReturnsAsync(recipeIds);
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(recipeDto);
            
            var result = await _controller.GetRecipesWithRestrictionsAsync(restrictionIds);
            var resultValue = result.Result as OkObjectResult;
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That(resultValue!.Value, Is.InstanceOf<IEnumerable<RecipeDto>>());
            });
        }

        [Test]
        public async Task UpdateRecipeAsync_IdNotFound_ShouldReturnNotFound()
        {
            var invalidId = 404;

            var recipeDto = new RecipeDto
            {
                Id = invalidId,
            };
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(invalidId))
                .ReturnsAsync(null as RecipeDto);
            
            var result = await _controller.UpdateRecipeAsync(invalidId, recipeDto);
            
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdateRecipeAsync_RecipeWithSameName_ShouldReturnOk()
        {
            var id = 200;

            var recipeDto = new RecipeDto
            {
                Id = id,
                Name = "Foo",
                Description = "Bar",
                Subrecipes = [],
                ParentRecipes = []
            };

            var newRecipeDto = new RecipeDto()
            {
                Id = id,
                Name = "Foo",
                Description = "Foo",
                Subrecipes = [],
                ParentRecipes = [],
            };
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(id))
                .ReturnsAsync(recipeDto);
            
            var result = await _controller.UpdateRecipeAsync(newRecipeDto.Id, newRecipeDto);
            var resultValue = result.Result as OkObjectResult;
            
            Assert.Multiple( () =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo(newRecipeDto));
            });
        }

        [Test]
        public async Task UpdateRecipeAsync_ReturnsOk()
        {
            var id = 200;

            var recipeDto = new RecipeDto
            {
                Id = id,
                Name = "Foo",
                Description = "Bar",
                Subrecipes = [],
                ParentRecipes = []
            };

            var newRecipeDto = new RecipeDto()
            {
                Id = id,
                Name = "Bar",
                Description = "Foo",
                Subrecipes = [],
                ParentRecipes = [],
            };
            
            _recipeRepositoryMock.Setup(r => r.GetRecipeByIdAsync(id))
                .ReturnsAsync(recipeDto);
            
            var result = await _controller.UpdateRecipeAsync(newRecipeDto.Id, newRecipeDto);
            var resultValue = result.Result as OkObjectResult;
            
            Assert.Multiple( () =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                Assert.That(resultValue!.Value, Is.EqualTo(newRecipeDto));
            });
        }
    }
}
