using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int orderId);
        //Task SaveChangesAsync();

        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> UpdateRiderAssignmentAsync(int orderId, int riderId);
    }
}
