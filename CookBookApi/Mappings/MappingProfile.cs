using AutoMapper;
using CookBookApi.DTOs;
using CookBookApi.DTOs.Ingredient;
using CookBookApi.DTOs.MeasurementUnit;
using CookBookApi.DTOs.Recipes;
using CookBookApi.Models;

namespace CookBookApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Ingredient, IngredientDto>()
                .ForMember(dest => dest.Cuisine, opt => opt.MapFrom(src => src.Cuisine))
                .ForMember(dest => dest.Restrictions, opt => opt.MapFrom(src => src.IngredientRestrictions.Select(ir => ir.Restriction.Name)));

            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.Subrecipes, opt => opt.MapFrom(src => src.Subrecipes))
                .ForMember(dest => dest.Cuisine, opt => opt.MapFrom(src => src.Cuisine))
                .ForMember(dest => dest.Restrictions, opt => opt.MapFrom(src => src.RecipeRestrictions.Select(rr => rr.Restriction)))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients));

            CreateMap<RecipeIngredient, RecipeIngredientDto>()
           .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.Name))
           .ForMember(dest => dest.MeasurementUnit, opt => opt.MapFrom(src => src.MeasurementUnit.Abbreviation));

            CreateMap<Restriction, RestrictionDto>();
            CreateMap<Cuisine, CuisineDto>();
                      
            CreateMap<AddIngredientDto, Ingredient>();
            CreateMap<AddRecipeDto, Recipe>();
            CreateMap<AddMeasurementUnitDto, MeasurementUnit>();

            CreateMap<MeasurementUnit, MeasurementUnitDto>();
            
            CreateMap<RecipeImage, RecipeImageDto>();
        }
    }
}
