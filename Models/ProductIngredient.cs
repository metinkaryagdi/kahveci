using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kahveci.Models
{
    public class ProductIngredient
    {
        [Key]
        public int ProductIngredientId { get; set; }

        public int ProductId { get; set; }
        public Products Product { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public decimal Quantity { get; set; }  // Kullanılan malzeme miktarı (örneğin, 100g süt)
    }
}
