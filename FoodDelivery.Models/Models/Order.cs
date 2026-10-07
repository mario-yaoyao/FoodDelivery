using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.Models.Models
{
       public class Order : Base
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;

        public User User { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public ICollection<Payment> Payments { get; set; } = [];
        public ICollection<DeliveryAssignment> DeliveryAssignments { get; set; } = [];
    }
}
