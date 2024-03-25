using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Recipes.API.Models;
using Recipes.API.Services;

namespace Recipes.API.Controllers
{
    /// <summary>
    /// API to get, create, update, and delete recipes from a cookbook.
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/recipes")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        private readonly ILogger<RecipesController> logger;
        private readonly IMapper mapper;

        public RecipesController(ILogger<RecipesController> logger,
            IRecipeService recipeService,
            IMapper mapper)
        {
            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            this.recipeService = recipeService ??
                throw new ArgumentNullException(nameof(recipeService));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a list of all recipes
        /// </summary>
        /// <returns>List of RecipeWithoutIngredientsDtos</returns>
        /// <response code="200">Returns list of recipes</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecipeWithoutIngredientsDto>>> GetRecipes()
        {
                return Ok(await recipeService.GetRecipesAsync());
        }

        /// <summary>
        /// Get requested recipe
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <param name="includeIngredients">Whether to include recipes ingredients</param>
        /// <returns>Requested recipe with or without ingredient list</returns>
        /// <response code="200">Returns requested recipe</response>
        /// <response code="404">Requested recipe was not found</response>
        [HttpGet("{recipeId}", Name = "GetRecipe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecipe(int recipeId,
            bool includeIngredients = false)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            if (includeIngredients)
            {
                return Ok(await recipeService.GetRecipeWithIngredientsAsync(recipeId));
            }
            else
            {
                return Ok(await recipeService.GetRecipeWithoutIngredientsAsync(recipeId));
            }
        }

        /// <summary>
        /// Add a recipe to the cookbook
        /// </summary>
        /// <param name="recipe">RecipeForCreationDto to be added</param>
        /// <returns>Added RecipeDto</returns>
        /// <response code="201">Recipe successfully added</response>
        /// <response code="400">Missing required fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RecipeDto>> AddRecipe(RecipeForCreationDto recipe)
        {
            var newRecipe = await recipeService.AddRecipeAsync(recipe);

            return CreatedAtRoute("GetRecipe",
                new
                {
                    recipeId = newRecipe.Id
                },
                newRecipe);
        }

        /// <summary>
        /// Update an existing recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="recipe">RecipeForUpdateDto to be updated</param>
        /// <returns>No content</returns>
        /// <response code="204">Recipe successfully updated</response>
        /// <response code="400">Missing required fields</response>
        [HttpPut("{recipeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateRecipe(int recipeId,
            RecipeForUpdateDto recipe)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            await recipeService.UpdateRecipeAsync(recipeId, recipe);

            return NoContent();
        }

        /// <summary>
        /// Partially update an existing recipe
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <param name="patchDocument">JSON patch command</param>
        /// <returns>No content.</returns>
        /// <response code="204">Recipe successfully updated</response>
        /// <response code="400">Missing required fields</response>
        [HttpPatch("{recipeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateRecipe(int recipeId,
            JsonPatchDocument<RecipeForUpdateDto> patchDocument)
        {
            var recipeEntity = await recipeService.GetRecipeEntityAsync(recipeId);
            if (recipeEntity == null)
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            var recipeToPatch = mapper.Map<RecipeForUpdateDto>(recipeEntity);

            patchDocument.ApplyTo(recipeToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(recipeToPatch))
            {
                return BadRequest(ModelState);
            }

            await recipeService.UpdateRecipeAsync(recipeToPatch, recipeEntity);

            return NoContent();
        }

        /// <summary>
        /// Delete a recipe from cookbook
        /// </summary>
        /// <param name="recipeId">Id of recipe</param>
        /// <returns>No content</returns>
        /// <response code="204">Recipe successfully deleted</response>
        /// <response code="404">Recipe not found</response>
        [HttpDelete("{recipeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRecipe(int recipeId)
        {
            if (!await recipeService.RecipeExistsAsync(recipeId))
            {
                logger.LogInformation("RecipeId {recipeId} was not found.", recipeId);
                return NotFound();
            }

            recipeService.DeleteRecipeAsync(recipeId);

            return NoContent();
        }
    }
}
