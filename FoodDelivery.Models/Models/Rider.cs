using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.Models.Models
{

    public class Rider : Base
    {
        public string FullName { get; set; } = string.Empty;
        public RiderStatus Status { get; set; } = RiderStatus.Available;

        public ICollection<DeliveryAssignment> DeliveryAssignments { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Analytic> Analytics { get; set; } = [];
    }
}
