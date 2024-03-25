using Recipes.API.Entities;

namespace Recipes.API.Models
{
    public class RecipeWithoutIngredientsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Directions { get; set; }
        public string? Difficulty { get; set; }
        public short? Rating { get; set; }
    }
}
