namespace FoodDelivery.Shared.Contracts.Events
{
    public class PaymentRequestedEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
