namespace FoodDelivery.Models.Models
{
    public class DeliveryAssignment : Base
    {
        public int OrderId { get; set; }
        public int RiderId { get; set; }

        public Order Order { get; set; } = null!;
        public Rider Rider { get; set; } = null!;
    }
}
