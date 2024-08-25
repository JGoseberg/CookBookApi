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
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.MeasurementUnit.Name))
                .ForMember(dest => dest.Cuisine, opt => opt.MapFrom(src => src.Cuisine))
                .ForMember(dest => dest.Restrictions, opt => opt.MapFrom(src => src.IngredientRestrictions.Select(ir => ir.Restriction)));

            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Subrecipes, opt => opt.MapFrom(src => src.Subrecipes))
                .ForMember(dest => dest.Cuisine, opt => opt.MapFrom(src => src.Cuisine))
                .ForMember(dest => dest.Restrictions, opt => opt.MapFrom(src => src.RecipeRestrictions.Select(rr => rr.Restriction)));

            CreateMap<Restriction, RestrictionDto>();
            CreateMap<Cuisine, CuisineDto>();

        }
    }
}
