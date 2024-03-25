namespace Recipes.API.Models
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Directions { get; set; }
        public string? Difficulty { get; set; }
        public short? Rating { get; set; }
        public int? NumberOfIngredients
        {
            get
            {
                return Ingredients.Count;
            }
        }
        public ICollection<IngredientDto> Ingredients { get; set; }
        = new List<IngredientDto>();
    }
}
