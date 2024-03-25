using AutoMapper;
using Recipes.API.Data;
using Recipes.API.Entities;
using Recipes.API.Models;

namespace Recipes.API.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ILogger<RecipeService> logger;
        private readonly IRecipeInfoRepository recipeInfoRepository;
        private readonly IMapper mapper;

        public RecipeService(ILogger<RecipeService> logger,
            IRecipeInfoRepository recipeInfoRepository,
            IMapper mapper)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            this.recipeInfoRepository = recipeInfoRepository ??
                throw new ArgumentNullException(nameof(recipeInfoRepository));
            this.mapper = mapper;
        }

        public async Task<Recipe> GetRecipeEntityAsync(int recipeId)
        {
            return await recipeInfoRepository.GetRecipeAsync(recipeId);
        }

        public async Task<IEnumerable<RecipeWithoutIngredientsDto>> GetRecipesAsync()
        {
            var recipes = await recipeInfoRepository.GetRecipesAsync();
            return mapper.Map<IEnumerable<RecipeWithoutIngredientsDto>>(recipes);
        }

        public async Task<RecipeWithoutIngredientsDto> GetRecipeWithoutIngredientsAsync(int recipeId)
        {
            var recipe = await recipeInfoRepository.GetRecipeAsync(recipeId);
            return mapper.Map<RecipeWithoutIngredientsDto>(recipe);
        }

        public async Task<RecipeDto> GetRecipeWithIngredientsAsync(int recipeId)
        {
            var recipe = await recipeInfoRepository.GetRecipeWithIngredientsAsync(recipeId);
            return mapper.Map<RecipeDto>(recipe);
        }

        public async Task<bool> RecipeExistsAsync(int recipeId)
        {
            return await recipeInfoRepository.RecipeExistsAsync(recipeId);
        }

        public async Task<RecipeDto> AddRecipeAsync(RecipeForCreationDto recipeForCreation)
        {
            var recipeEntity = mapper.Map<Recipe>(recipeForCreation);
            await recipeInfoRepository.AddRecipeAsync(recipeEntity);
            await recipeInfoRepository.SaveChangesAsync();
            return mapper.Map<RecipeDto>(recipeEntity);
        }

        public async Task<bool> UpdateRecipeAsync(int recipeId, RecipeForUpdateDto recipeUpdate)
        {
            var recipeEntity = await recipeInfoRepository.GetRecipeAsync(recipeId);
            mapper.Map(recipeUpdate, recipeEntity);
            return await recipeInfoRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateRecipeAsync(RecipeForUpdateDto recipeUpdate, Recipe recipeEntity)
        {
            mapper.Map(recipeUpdate, recipeEntity);
            return await recipeInfoRepository.SaveChangesAsync();
        }

        public async void DeleteRecipeAsync(int recipeId)
        {
            var recipeEntity = await recipeInfoRepository.GetRecipeAsync(recipeId);
            recipeInfoRepository.DeleteRecipe(recipeEntity);
            await recipeInfoRepository.SaveChangesAsync();
        }
    }
}
