namespace Recipes.API.Models
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Qty { get; set; }
        public string? Measurement { get; set; }
    }
}
