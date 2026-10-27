using FoodDelivery.Protos;

namespace FoodDelivery.BLL.Interfaces;

public interface IAnalyticsService
{
    Task<int> BulkInsertAsync(List<AnalyticsRequest> requests);
}