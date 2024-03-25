using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Recipes.API.Models;
using Recipes.API.Services;

namespace Recipes.API.Controllers
{
    /// <summary>
    /// API to get, add, update, and delete ingredients from recipes.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/recipes/{recipeId}/ingredients")]
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

        /// <summary>
        /// Get a recipes ingredients
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <returns>List of IngredientDtos</returns>
        /// <response code="200">Returns requested recipes ingredients</response>
        /// <response code="404">Requested recipe was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Get specific ingredient for a recipe
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <param name="ingredientId">Id of ingredient</param>
        /// <returns>IngredientDto</returns>
        /// <response code="200">Returns requested recipes specific ingredient</response>
        /// <response code="404">Requested recipe or ingredient was not found</response>
        [HttpGet("{ingredientId}", Name = "GetIngredient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Add an ingredient to a recipe
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <param name="ingredient">IngredientForCreationDto to be added</param>
        /// <returns>IngredientDto of added ingredient</returns>
        /// <response code="201">Ingredient succssfully added</response>
        /// <response code="404">Requested recipe was not found</response>
        /// <response code="400">Missing required field</response>
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

        /// <summary>
        /// Update an ingredient for a recipe
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <param name="ingredientId">Id of ingredient</param>
        /// <param name="ingredientUpdate">IngredientForUpdateDto to be updated</param>
        /// <returns>No content</returns>
        /// <response code="204">Ingredient succssfully updated</response>
        /// <response code="404">Requested recipe or ingredient was not found</response>
        /// <response code="400">Missing required field</response>
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

        /// <summary>
        /// Delete a recipes ingredient
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <param name="ingredientId">Id of ingredient</param>
        /// <returns>No content</returns>
        /// <response code="204">Ingredient succssfully updated</response>
        /// <response code="404">Requested recipe or ingredient was not found</response>
        [HttpDelete("{ingredientId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
