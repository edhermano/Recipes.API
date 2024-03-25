using AutoMapper;

namespace Recipes.API.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile() 
        {
            CreateMap<Entities.Recipe, Models.RecipeWithoutIngredientsDto>();
            CreateMap<Entities.Recipe, Models.RecipeDto>();
            CreateMap<Entities.Recipe, Models.RecipeForUpdateDto>();
            CreateMap<Models.RecipeForCreationDto, Entities.Recipe>();
            CreateMap<Models.RecipeForUpdateDto, Entities.Recipe>();
        }
    }
}
