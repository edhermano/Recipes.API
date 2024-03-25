using System.ComponentModel.DataAnnotations;

namespace Recipes.API.Models
{
    public class IngredientForCreationDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Qty { get; set; }
        [MaxLength(25)]
        public string? Measurement { get; set; }
    }
}
