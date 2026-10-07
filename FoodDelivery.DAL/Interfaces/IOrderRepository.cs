using FoodDelivery.Models.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task SaveChangesAsync();
    }
}
