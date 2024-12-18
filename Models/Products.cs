using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kahveci.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ProductType { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public ICollection<OrderDetails> OrderDetailsId { get; set; } = new List<OrderDetails>();
        public ICollection<Favorites> FavoritesId { get; set; } = new List<Favorites>();

        public int ProductCategoriesId { get; set; }

        [ForeignKey("ProductCategoriesId")]
        public ProductCategories ProductCategories { get; set; }

        // Ürüne ait malzemelerin listesi
        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
    }

}
