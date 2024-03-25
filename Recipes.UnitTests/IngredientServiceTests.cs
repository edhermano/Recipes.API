using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Recipes.API.Data;
using Recipes.API.Entities;
using Recipes.API.Models;
using Recipes.API.Services;

namespace Recipes.UnitTests
{
    public class IngredientServiceTests
    {
        private Mock<IRecipeInfoRepository> mockRecipeInfoRepository;
        private Mock<ILogger<IngredientService>> mockLogger;
        private Mock<IMapper> mockMapper;
        private IngredientService ingredientService;

        [SetUp]
        public void Setup()
        {
            mockRecipeInfoRepository = new Mock<IRecipeInfoRepository>();
            mockLogger = new Mock<ILogger<IngredientService>>();
            mockMapper = new Mock<IMapper>();

            ingredientService = new IngredientService(mockLogger.Object, mockRecipeInfoRepository.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetIngredientsForRecipeAsync_CallsRepo_Once()
        {
            var recipeId = 3;

            var result = await ingredientService.GetIngredientsForRecipeAsync(recipeId);

            mockRecipeInfoRepository.Verify(x => x.GetIngredientsForRecipeAsync(
                It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task GetIngredientAsync_CallsRepo_Once()
        {
            var recipeId = 3;
            var ingredientId = 7;

            var result = await ingredientService
                .GetIngredientAsync(recipeId, ingredientId);

            mockRecipeInfoRepository.Verify(x => x.GetIngredientForRecipeAsync(
                It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task AddIngredientToRecipeAsync_CallsRepo_Twice()
        {
            var recipeId = 3;

            IngredientForCreationDto ingredient = new IngredientForCreationDto()
            {
                Name = "Test",
                Qty = 1,
                Measurement = "test"
            };

            var result = await ingredientService
                .AddIngredientToRecipeAsync(recipeId, ingredient);

            mockRecipeInfoRepository.Verify(x => x.AddIngredientToRecipeAsync(
                It.IsAny<int>(), It.IsAny<Ingredient>()), Times.Once());
            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Test]
        public async Task UpdateIngredientAsync_CallsRepo_Twice()
        {
            var recipeId = 3;
            var ingredientId = 7;

            IngredientForUpdateDto ingredient = new IngredientForUpdateDto()
            {
                Name = "Test",
                Qty = 1,
                Measurement = "test"
            };

            var result = await ingredientService
                .UpdateIngredientAsync(recipeId, ingredientId, ingredient);

            mockRecipeInfoRepository.Verify(x => x.GetIngredientForRecipeAsync(
                It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }

        [Test]
        public void DeleteIngredientAsync_CallsRepo_Twice()
        {
            var recipeId = 3;
            var ingredientId = 7;

            ingredientService.DeleteIngredientAsync(recipeId, ingredientId);

            mockRecipeInfoRepository.Verify(x => x.GetIngredientForRecipeAsync(
                It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            mockRecipeInfoRepository.Verify(x => x.SaveChangesAsync(),
                Times.Once());
        }
    }
}
