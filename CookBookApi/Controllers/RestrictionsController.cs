using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CookBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestrictionsController : ControllerBase
    {
        private readonly IRestrictionRepository _restrictionRepository;
        private readonly IMapper _mapper;

        public RestrictionsController(IRestrictionRepository restrictionRepository, IMapper mapper)
        {
            _restrictionRepository = restrictionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestrictionDto>>> GetAllRestrictionsAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestrictionDto>> GetRestrictionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> AddRestrictionAsync(RestrictionDto restrictionDto)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<RestrictionDto>> UpdateRestrictionAsync(int id, RestrictionDto restrictionDto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestrictionAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
