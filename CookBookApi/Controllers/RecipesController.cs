﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookBookApi.Data;
using CookBookApi.Models;

namespace CookBookApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : Controller
    {
        private readonly CookBookContext _context;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(CookBookContext context, ILogger<RecipesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Recipes
        [HttpGet(Name = "GetRecipes")]
        public async Task<ActionResult<List<Recipe>>> Get()
        {
            return await _context.Recipes.Include(r => r.Ingredients).ToListAsync();
        }

        [HttpPost(Name = "PostRecipes")]
        public async Task<ActionResult<List<Ingredient>>> PostRecipe(int ingredient)
        {
            var ingredients = await _context.Ingredients.ToListAsync();

            Ingredient ingredientToAdd = ingredients.Where(x => x.Id == ingredient).First();


            _context.Recipes.Add(
                new Recipe
                {
                    Name = "test",
                    Ingredients =
                    {
                        ingredientToAdd
                    }
                });
            _context.SaveChanges();

            return Ok();
        }


    }
}
