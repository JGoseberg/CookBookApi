using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Interfaces;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IMapper _mapper;

    public RecipesController(IRecipeRepository recipeRepository, IMapper mapper)
    {
        _mapper = mapper;
        _recipeRepository = recipeRepository;
    }

    [HttpPost]
    public async Task<ActionResult> AddRecipeAsync(AddRecipeDto recipeDto)
    {
        // TODO NameIsEmpty
        // TODO SameName

        // TODO AutoAddIngredient
        // TODO AutoAddRestriction
        // TODO AutoAddCuisines

        throw new NotImplementedException();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipeAsync(int id)
    {
        // TODO NotExisting

        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<RecipeDto>> GetRandomRecipeAsync()
    {
        // TODO rnd -> GetbyID 

        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllRecipesAsync()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithCuisinesAsync(List<CuisineDto> cuisineDtos)
    {
        // TODO cuisines == empty -> GetAll
        // TODO no recipes Found
        // TODO cuisine not exists

        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<RecipeDto>> GetRecipesWithIngredientsAsync(List<IngredientDto> ingredients)
    {
        // TODO ingredients == empty -> GetAll
        // TODO no recipes Found
        // TODO ingredient not exists

        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetRecipeByIdAsync(int id)
    {
        // TODO not Found

        throw new NotImplementedException();
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<RecipeDto>> UpdateRecipeAsync(int id, RecipeDto recipeDto)
    {
        // TODO existing name
        // TODO not exists

        throw new NotImplementedException();
    }

}
