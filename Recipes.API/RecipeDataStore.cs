using Recipes.API.Models;

namespace Recipes.API
{
    public class RecipeDataStore
    {
        public List<RecipeDto> Recipes { get; set; }

        public static RecipeDataStore Instance { get; set; } = new RecipeDataStore();

        public RecipeDataStore()
        {
            Recipes = new List<RecipeDto>()
            {
                new RecipeDto()
                {
                    Id = 1,
                    Name = "Cajun Meatloaf",
                    Directions= "Mix bake serve",
                    Difficulty = "Easy",
                    Rating = 5,
                    Ingredients = new List<IngredientDto>()
                    {
                        new IngredientDto()
                        {
                            Id = 1,
                            Name = "Ground Beef",
                            Qty = 2,
                            Measurement = "lbs"

                        },
                        new IngredientDto()
                        {
                            Id = 2,
                            Name = "Egg",
                            Qty= 2
                        }
                    }
                },
                new RecipeDto()
                {
                    Id = 2,
                    Name = "Mexican Lasagna",
                    Directions= "Mix bake serve",
                    Difficulty = "Easy",
                    Rating = 5
                },
                new RecipeDto()
                {
                    Id = 3,
                    Name = "Enchilades",
                    Directions= "Mix bake serve",
                    Difficulty = "Easy",
                    Rating = 5
                }
            };
        }
    }
}
