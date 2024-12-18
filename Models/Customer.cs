using System.ComponentModel.DataAnnotations;

namespace Kahveci.Models
{
    public class Customer
    {
        [Key]

        public int CustomerId { get; set; }
        public String? CustomerName { get; set; } = string.Empty;
        public String? CustomerSurname { get; set; } = string.Empty;
        public String? CustomerEmail { get; set; } = string.Empty;
        public String? CustomerAdress { get; set; } = string.Empty;
        public String? CustomerPhone { get; set; } = string.Empty;
        public String? Password { get; set; } = string.Empty;
        public int CoffeeBeans { get; set; }
        public int FreeCoffees { get; set; }
        public ICollection<Orders> orders { get; set; } = new List<Orders>();
        public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();
    }
}