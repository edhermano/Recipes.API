using AutoMapper;
using Recipes.API.Data;
using Recipes.API.Entities;
using Recipes.API.Models;

namespace Recipes.API.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ILogger<IngredientService> logger;
        private readonly IRecipeInfoRepository recipeInfoRepository;
        private readonly IMapper mapper;

        public IngredientService(ILogger<IngredientService> logger,
            IRecipeInfoRepository recipeInfoRepository,
            IMapper mapper)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            this.recipeInfoRepository = recipeInfoRepository ?? 
                throw new ArgumentNullException(nameof(recipeInfoRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<IngredientDto>> GetIngredientsForRecipeAsync(int recipeId)
        {
            var ingredients = await recipeInfoRepository
                .GetIngredientsForRecipeAsync(recipeId);
            return mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public async Task<IngredientDto> GetIngredientAsync(int recipeId, int ingredientId)
        {
            var ingredient = await recipeInfoRepository
                .GetIngredientForRecipeAsync(recipeId, ingredientId);
            return mapper.Map<IngredientDto>(ingredient);
        }

        public async Task<IngredientDto> AddIngredientToRecipeAsync(int recipeId,
            IngredientForCreationDto ingredient)
        {
            var ingredientEntity = mapper.Map<Ingredient>(ingredient);
            await recipeInfoRepository.AddIngredientToRecipeAsync(recipeId,
                ingredientEntity);
            await recipeInfoRepository.SaveChangesAsync();
            return mapper.Map<IngredientDto>(ingredientEntity);
        }

        public async Task<bool> UpdateIngredientAsync(int recipeId,
            int ingredientId,
            IngredientForUpdateDto ingredientUpdate)
        {
            var ingredientEntity = await recipeInfoRepository
                .GetIngredientForRecipeAsync(recipeId, ingredientId);
            mapper.Map(ingredientUpdate, ingredientEntity);
            return await recipeInfoRepository.SaveChangesAsync();
        }

        public async void DeleteIngredientAsync(int recipeId, int ingredientId)
        {
            var ingredientEntity = await recipeInfoRepository
                .GetIngredientForRecipeAsync(recipeId, ingredientId);
            recipeInfoRepository.DeleteIngredient(ingredientEntity);
            await recipeInfoRepository.SaveChangesAsync();
        }
    }
}
