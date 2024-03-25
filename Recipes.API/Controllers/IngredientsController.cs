using Microsoft.AspNetCore.Mvc;
using Recipes.API.Models;
using Recipes.API.Services;

namespace Recipes.API.Controllers
{
    [ApiController]
    [Route("recipes/{recipeId}/ingredients")]
    public class IngredientsController : ControllerBase
    {

        private readonly ILogger<IngredientsController> logger;
        private readonly IIngredientService ingredientService;
        private readonly IRecipeService recipeService;

        public IngredientsController(ILogger<IngredientsController> logger,
            IIngredientService ingredientService,
            IRecipeService recipeService) 
        { 
            this.logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            this.ingredientService = ingredientService ??
                throw new ArgumentNullException(nameof(ingredientService));
            this.recipeService = recipeService ??
                throw new ArgumentNullException(nameof(recipeService));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients(int recipeId)
        {
            var ingredients = await ingredientService.GetIngredientsForRecipeAsync(recipeId);

            if (ingredients == null)
            {
                logger.LogInformation("No ingredients found for recipeId: {recipeId}", recipeId);
                return NotFound();
            }

            return Ok(ingredients);
        }

        [HttpGet("{ingredientId}", Name = "GetIngredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IngredientDto>> GetIngredient(int recipeId, int ingredientId)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            var ingredient = await ingredientService.GetIngredientAsync(recipeId, ingredientId);
            if (ingredient == null)
            {
                logger.LogInformation("Ingredient not found for recipeId: {recipeId}", recipeId);
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IngredientDto>> AddIngredient(int recipeId,
            IngredientForCreationDto ingredient)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            var newIngredient = ingredientService.AddIngredientToRecipeAsync(recipeId, ingredient);

            return CreatedAtRoute("GetIngredient", 
                new
                {
                    recipeId = recipeId,
                    ingredientId = newIngredient.Id
                },
                newIngredient);
        }

        [HttpPut("{ingredientId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateIngredient(int recipeId,
            int ingredientId,
            IngredientForUpdateDto ingredientUpdate)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            var ingredient = await ingredientService.GetIngredientAsync(recipeId, ingredientId);
            if (ingredient == null)
            {
                logger.LogInformation("Ingredient not found for recipeId: {recipeId}", recipeId);
                return NotFound();
            }

            await ingredientService.UpdateIngredientAsync(recipeId, ingredientId, ingredientUpdate);

            return NoContent();
        }

        [HttpDelete("{ingredientId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteIngredient(int recipeId, int ingredientId)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            var ingredient = await ingredientService.GetIngredientAsync(recipeId, ingredientId);
            if (ingredient == null)
            {
                logger.LogInformation("Ingredient not found for recipeId: {recipeId}", recipeId);
                return NotFound();
            }

            ingredientService.DeleteIngredientAsync(recipeId, ingredientId);

            return NoContent();
        }
    }
}
