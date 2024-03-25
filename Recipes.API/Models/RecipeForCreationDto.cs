using System.ComponentModel.DataAnnotations;

namespace Recipes.API.Models
{
    public class RecipeForCreationDto
    {
        [Required(ErrorMessage = "This recipe needs a name!")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Directions { get; set; }
        public string? Difficulty { get; set; }
        public short? Rating { get; set; }
        public ICollection<IngredientForCreationDto> Ingredients { get; set; }
        = new List<IngredientForCreationDto>();
    }
}
