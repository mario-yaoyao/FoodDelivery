namespace FoodDelivery.Models.Models
{
    public class Restaurant : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<MenuItem> MenuItems { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Analytic> Analytics { get; set; } = [];
    }
}
