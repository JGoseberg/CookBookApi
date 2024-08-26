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
        
        public RestrictionsController(IRestrictionRepository restrictionRepository)
        {
            _restrictionRepository = restrictionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestrictionDto>>> GetRestrictionsAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestrictionDto>> GetRestrictionById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<RestrictionDto>> PostRestrictionAsync(int id, RestrictionDto restrictionDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult> PutRestrictionAsync(RestrictionDto restrictionDto)
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
