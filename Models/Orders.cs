using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kahveci.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set;}
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set;}
        public String Status { get; set; }=string.Empty;
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer customer { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    
    }
}