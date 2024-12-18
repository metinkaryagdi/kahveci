using System.ComponentModel.DataAnnotations;

namespace Kahveci.Models
{
    public class ProductCategories
    {
        [Key]
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }=string.Empty;
        public String Description { get; set; }=string.Empty; 
        public ICollection<Products> productId { get; set; }
    }
}