using Recipes.API.Models;

namespace Recipes.API.Services
{
    public interface IIngredientService
    {
        Task<IngredientDto> AddIngredientToRecipeAsync(int recipeId, IngredientForCreationDto ingredient);
        void DeleteIngredientAsync(int recipeId, int ingredientId);
        Task<IngredientDto> GetIngredientAsync(int recipeId, int ingredientId);
        Task<IEnumerable<IngredientDto>> GetIngredientsForRecipeAsync(int recipeId);
        Task<bool> UpdateIngredientAsync(int recipeId, int ingredientId, IngredientForUpdateDto ingredientUpdate);
    }
}
