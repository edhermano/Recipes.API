using Microsoft.EntityFrameworkCore;
using Recipes.API.DbContexts;
using Recipes.API.Entities;

namespace Recipes.API.Data
{
    public class RecipeInfoRepository : IRecipeInfoRepository
    {
        private readonly RecipeInfoContext context;

        public RecipeInfoRepository(RecipeInfoContext dbContext)
        {
            context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Ingredient?> GetIngredientForRecipeAsync(int recipeId, int ingredientId)
        {
            return await context.Ingredients
                .Where(i => i.RecipeId == recipeId && i.Id == ingredientId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsForRecipeAsync(int recipeId)
        {
            return await context.Ingredients.Where(i => i.RecipeId == recipeId)
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeAsync(int recipeId)
        {
            return await context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);
        }

        public async Task<Recipe?> GetRecipeWithIngredientsAsync(int recipeId)
        {
            return await context.Recipes.Include(i => i.Ingredients)
                .Where(r => r.Id == recipeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await context.Recipes.ToListAsync();
        }

        public async Task<bool> RecipeExistsAsync(int recipeId)
        {
            return await context.Recipes.AnyAsync(r => r.Id == recipeId);
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await context.Recipes.AddAsync(recipe);
        }

        public void DeleteRecipe(Recipe recipe)
        {
            context.Recipes.Remove(recipe);
        }

        public void DeleteIngredient(Ingredient ingredient)
        {
            context.Ingredients.Remove(ingredient);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }

        public async Task AddIngredientToRecipeAsync(int recipeId, Ingredient ingredient)
        {
            var recipe = await GetRecipeAsync(recipeId);
            recipe?.Ingredients.Add(ingredient);
        }
    }
}
