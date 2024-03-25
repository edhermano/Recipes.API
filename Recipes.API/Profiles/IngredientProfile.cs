using AutoMapper;

namespace Recipes.API.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile() 
        {
            CreateMap<Entities.Ingredient, Models.IngredientDto>();
            CreateMap<Models.IngredientForCreationDto, Entities.Ingredient>();
            CreateMap<Models.IngredientForUpdateDto, Entities.Ingredient>();
        }
    }
}
