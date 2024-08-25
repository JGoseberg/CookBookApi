using CookBookApi.DTOs;
using CookBookApi.Interfaces.Repositories;
using CookBookApi.Models;

namespace CookBookApi.Repositories
{
    public class CuisineRepository : ICuisineRepository
    {
        public Task AddCuisineAsync(Cuisine cuisine)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCuisineAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CuisineDto> GetCuisineByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCuisineAsync(Cuisine cuisine)
        {
            throw new NotImplementedException();
        }
    }
}
