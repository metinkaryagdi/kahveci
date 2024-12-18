using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Kahveci.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int UserId { get; set; } 

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
