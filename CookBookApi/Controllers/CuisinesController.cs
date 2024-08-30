using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisinesController
    {
        private readonly ICuisineRepository _cuisineRepository;
        private readonly IMapper _mapper;

        public CuisinesController(ICuisineRepository cuisineRepository, IMapper mapper)
        {
            _cuisineRepository = cuisineRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuisineDto>>> GetCuisinesAsync()
        {
            var cuisines = await _cuisineRepository.GetAllCuisinesAsync();

            return cuisines.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuisineDto>> GetCuisineById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> AddCuisineAsync(CuisineDto cuisineDto)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<CuisineDto>> UpdateCuisineAsync(int id, CuisineDto cuisineDto)
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
