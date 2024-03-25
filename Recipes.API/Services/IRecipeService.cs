using AutoMapper;
using Recipes.API.Data;
using Recipes.API.Entities;
using Recipes.API.Models;

namespace Recipes.API.Services
{
    public interface IRecipeService
    {
        public Task<IEnumerable<RecipeWithoutIngredientsDto>> GetRecipesAsync();
        public Task<RecipeWithoutIngredientsDto> GetRecipeWithoutIngredientsAsync(int recipeId);
        public Task<RecipeDto> GetRecipeWithIngredientsAsync(int recipeId);
        public Task<bool> RecipeExistsAsync(int recipeId);
        public Task<RecipeDto> AddRecipeAsync(RecipeForCreationDto recipe);
        public Task<bool> UpdateRecipeAsync(int recipeID, RecipeForUpdateDto recipeUpdate);
        public Task<bool> UpdateRecipeAsync(RecipeForUpdateDto recipeUpdate, Recipe recipeEntity);
        public void DeleteRecipeAsync(int recipeId);
        public Task<Recipe> GetRecipeEntityAsync(int recipeId);

    }
}