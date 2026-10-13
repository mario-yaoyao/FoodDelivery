namespace FoodDelivery.Models.Models
{
    public class User : Base
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Analytic> Analytics { get; set; } = [];
    }
}
