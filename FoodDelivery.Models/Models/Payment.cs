using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.Models.Models
{
    public class Payment : Base
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentMethod Method { get; set; } = PaymentMethod.CashOnDelivery;

        public Order Order { get; set; } = null!;
    }
}
