namespace FoodDelivery.Shared.Contracts.Enums
{
    public enum OrderStatus
    {
        Created,
        Accepted,
        Preparing,
        ReadyForPickup,
        OutForDelivery,
        Delivered,
        Cancelled
    }
}
