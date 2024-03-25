using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Recipes.API.Entities
{
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Qty { get; set; }

        [MaxLength(25)]
        public string? Measurement { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe? Recipe { get; set; }
        public int RecipeId { get; set; }
    }
}
