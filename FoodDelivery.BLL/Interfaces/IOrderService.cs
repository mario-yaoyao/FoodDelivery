using FoodDelivery.Models.DTOs.Requests;
using FoodDelivery.Models.Models;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(int userId, CreateOrderReqDto order);
        Task<bool> MarkOrderReadyAsync(int orderId);
        Task<bool> ProcessPaymentAsync(int orderId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateRiderAssignmentAsync(int orderId, int riderId);
    }
}
