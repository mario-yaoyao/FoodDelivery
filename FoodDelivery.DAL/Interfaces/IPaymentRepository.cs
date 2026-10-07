using FoodDelivery.Models.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
    }
}
