using System.ComponentModel.DataAnnotations;

namespace Recipes.API.Models
{
    public class RecipeForUpdateDto
    {
        [Required(ErrorMessage = "This recipe needs a name!")]
        [MaxLength(50)]
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

