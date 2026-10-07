using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.Shared.Contracts.Events
{
    public class OrderStatusUpdatedEvent
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
