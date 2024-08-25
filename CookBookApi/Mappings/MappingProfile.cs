using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.Models;

namespace CookBookApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.MeasurementUnit.Name));

            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Subrecipes, opt => opt.MapFrom(src => src.Subrecipes));

            CreateMap<Cuisine, CuisineDto>();
        }
    }
}
