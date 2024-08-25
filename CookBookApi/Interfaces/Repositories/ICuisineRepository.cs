﻿using CookBookApi.DTOs;
using CookBookApi.Models;

namespace CookBookApi.Interfaces.Repositories
{
    public interface ICuisineRepository
    {
        Task<CuisineDto> GetCuisineByIdAsync(int id);
        Task<IEnumerable<CuisineDto>> GetAllCuisinesAsync();
        Task AddCuisineAsync(Cuisine cuisine);
        Task UpdateCuisineAsync(Cuisine cuisine);
        Task DeleteCuisineAsync(int id);
    }
}
