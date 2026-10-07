namespace FoodDelivery.Shared.Contracts.Events
{
    public class DeliveryRequestedEvent
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
