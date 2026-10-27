using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using FoodDelivery.Protos;
using FoodDelivery.Shared.Contracts.Enums;

namespace FoodDelivery.BLL.Services;

public class AnalyticsService(IAnalyticsRepository analyticsRepository) : IAnalyticsService
{
    public async Task<int> BulkInsertAsync(List<AnalyticsRequest> requests)
    {
        var analytics = requests.Select(request => new Analytic
        {
            OrderId = request.OrderId,
            CustomerId = request.CustomerId,
            RestaurantId = request.RestaurantId,
            TotalAmount = (decimal)request.TotalAmount,
            Status = Enum.Parse<OrderStatus>(request.Status),
            OrderedAt = DateTime.Parse(request.OrderedAt),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        }).ToList();

        var success = await analyticsRepository.BulkInsertAsync(analytics);

        return success ? analytics.Count : 0;
    }
}