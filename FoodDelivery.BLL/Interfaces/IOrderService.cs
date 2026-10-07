using FoodDelivery.Models.DTOs.Requests;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(int userId, CreateOrderReqDto order);
        Task<bool> MarkOrderReadyAsync(int orderId);
        Task<bool> ProcessPaymentAsync(int orderId);
    }
}
