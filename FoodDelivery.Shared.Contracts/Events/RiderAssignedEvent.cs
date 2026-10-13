namespace FoodDelivery.Shared.Contracts.Events
{
    public class RiderAssignedEvent
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
