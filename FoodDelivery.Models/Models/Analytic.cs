using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.Models.Models
{
    public class Analytic : Base
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int? RiderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public Order Order { get; set; } = null!;
        public User Customer { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
        public Rider? Rider { get; set; }
    }
}
