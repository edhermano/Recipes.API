using Recipes.API.Entities;

namespace Recipes.API.Data
{
    public interface IRecipeInfoRepository
    {
        public Task<IEnumerable<Recipe>> GetRecipesAsync();

        public Task<Recipe?> GetRecipeAsync(int recipeId);

        public Task<Recipe?> GetRecipeWithIngredientsAsync(int recipeId);

        public Task<Ingredient?> GetIngredientForRecipeAsync(int recipeId, int ingredientId);

        public Task<IEnumerable<Ingredient>> GetIngredientsForRecipeAsync(int recipeId);

        public Task<bool> RecipeExistsAsync(int recipeId);

        public Task AddRecipeAsync(Recipe recipe);

        public void DeleteRecipe(Recipe recipe);

        public Task<bool> SaveChangesAsync();
        Task AddIngredientToRecipeAsync(int recipeId, Ingredient ingredient);
        void DeleteIngredient(Ingredient ingredient);
    }
}
