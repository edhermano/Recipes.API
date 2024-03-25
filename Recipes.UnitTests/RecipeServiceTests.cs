using Recipes.API.Services;
using Moq;
using Recipes.API.Data;
using Serilog;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Recipes.API.Models;
using Recipes.API.Entities;

namespace Recipes.UnitTests
{
    public class RecipeServiceTests
    {

        private Mock<IRecipeInfoRepository> mockRecipeInfoRepository;
        private Mock<ILogger<RecipeService>> mockLogger;
        private Mock<IMapper> mockMapper;
        private RecipeService recipeService;

        [SetUp]
        public void Setup()
        {
            mockRecipeInfoRepository = new Mock<IRecipeInfoRepository>();
            mockLogger = new Mock<ILogger<RecipeService>>();
            mockMapper = new Mock<IMapper>();

            recipeService = new RecipeService(mockLogger.Object, mockRecipeInfoRepository.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetRecipesAsync_CallsRepo_Once()
        {
            var result = await recipeService.GetRecipesAsync();

            mockRecipeInfoRepository.Verify(x => x.GetRecipesAsync(), Times.Once());
        }

        [Test]
        public async Task GetRecipeWithoutIngredientsAsync_CallsRepo_Once()
        {
            var recipeId = 5;

            var result = await recipeService.GetRecipeWithoutIngredientsAsync(recipeId);

            mockRecipeInfoRepository.Verify(x => x.GetRecipeAsync(
                It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task GetRecipeWithIngredientsAsync_CallsRepo_Once()
        {
            var recipeId = 5;

            var results = await recipeService.GetRecipeWithIngredientsAsync(recipeId);

            mockRecipeInfoRepository.Verify(x => x.GetRecipeWithIngredientsAsync(
                It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task RecipeExistsAsync_CallsRepoOnce_ReturnsFalse()
        {
            var recipeId = 5;
            mockRecipeInfoRepository.Setup(x => x.RecipeExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await recipeService.RecipeExistsAsync(recipeId);

            mockRecipeInfoRepository.Verify(x => x.RecipeExistsAsync(
                It.IsAny<int>()), Times.Once());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AddRecipeAsync_CallsRepo_Twice()
        {
            RecipeForCreationDto recipe = new RecipeForCreationDto()
            {
                Name = "Test",
                Directions = "Some Directions",
                Difficulty = "Easy",
                Rating = 4
            };

            var result = await recipeService.AddRecipeAsync(recipe);

            mockRecipeInfoRepository.Verify(x => x.AddRecipeAsync(
                It.IsAny<Recipe>()), Times.Once());
            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Test]
        public async Task UpdateRecipeAsync_CallsRepo_Twice()
        {
            int recipeId = 7;
            RecipeForUpdateDto recipe = new RecipeForUpdateDto()
            {
                Name = "Test",
                Directions = "Some Directions",
                Difficulty = "Easy",
                Rating = 4
            };

            var result = await recipeService.UpdateRecipeAsync(recipeId, recipe);

            mockRecipeInfoRepository.Verify(x => x.GetRecipeAsync(
                It.IsAny<int>()), Times.Once());
            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Test]
        public async Task UpdateRecipeAsync_Overload_CallsRepo_Once()
        {
            RecipeForUpdateDto recipe = new RecipeForUpdateDto()
            {
                Name = "Test",
                Directions = "Some Directions",
                Difficulty = "Medium",
                Rating = 4
            };

            Recipe recipeEntity = new Recipe()
            {
                Id = 1,
                Name = "Test",
                Directions = "Some Directions",
                Difficulty = "Easy",
                Rating = 4
            };

            var result = await recipeService.UpdateRecipeAsync(recipe, recipeEntity);

            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Test]
        public void DeleteRecipeAsync_CallsRepo_Twice()
        {
            var recipeId = 3;

            recipeService.DeleteRecipeAsync(recipeId);

            mockRecipeInfoRepository.Verify(x => x.DeleteRecipe(It.IsAny<Recipe>()),
                Times.Once());
            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }
    }
}