using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kahveci.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set;}
        public int Quantity { get; set;}
        public decimal Price { get; set;}
        public decimal Subtotal => Quantity * Price;
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        [ForeignKey("ProductId")]
        public Products Products { get; set; }
        [ForeignKey("OrderId")]
        public Orders Order { get; set; }
   
   
    }
}