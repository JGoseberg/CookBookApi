using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisinesController
    {
        private readonly ICuisineRepository _cuisineRepository;

        public CuisinesController(ICuisineRepository cuisineRepository)
        {
            _cuisineRepository = cuisineRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuisineDto>>> GetCuisinesAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuisineDto>> GetCuisineById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<CuisineDto>> PostCuisineAsync(int id, CuisineDto cuisineDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult> PutCuisineAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCuisineAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
