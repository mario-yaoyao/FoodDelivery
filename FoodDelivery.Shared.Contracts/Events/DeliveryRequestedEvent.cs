namespace FoodDelivery.Shared.Contracts.Events
{
    public class DeliveryRequestedEvent
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
    }
}
