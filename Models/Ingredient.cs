using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kahveci.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;  // Malzeme adı (örneğin, kahve çekirdeği, süt)

        public string? Description { get; set; }  // Opsiyonel açıklama

        public decimal StockQuantity { get; set; }  // Malzemenin mevcut stok miktarı
    }
}
