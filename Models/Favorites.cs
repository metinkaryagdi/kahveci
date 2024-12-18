using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kahveci.Models
{
    public class Favorites
    {
        [Key]

        public int FavoriteId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Products Products { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
