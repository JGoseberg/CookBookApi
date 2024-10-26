using CookBookApi.DTOs;
using CookBookApi.Models;

namespace CookBookApi.Interfaces.Repositories
{
    public interface ICuisineRepository
    {
        Task<bool> AnyCuisineWithSameNameAsync(string name);
        Task AddCuisineAsync(Cuisine cuisine);
        Task DeleteCuisineAsync(int id);
        Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync();
        Task<CuisineDto?> GetCuisineByIdAsync(int id);
        Task UpdateCuisineAsync(Cuisine cuisine);
    }
}
