using Recipes.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.API.Entities
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
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

        public ICollection<Ingredient> Ingredients { get; set; }
                = new List<Ingredient>();
    }
}
