using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.Shared.Contracts.Events
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public int? RiderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
