using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Recipes.API.Models;
using Recipes.API.Services;

namespace Recipes.API.Controllers
{
    [ApiController]
    [Route("recipes")]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
        {
                return Ok(await recipeService.GetRecipesAsync());
        }

        [HttpGet("{recipeId}", Name = "GetRecipe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpPut("{recipeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPatch("{recipeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpDelete("{recipeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
