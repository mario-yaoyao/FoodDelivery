using FoodDelivery.Models.Models;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IAnalyticsService
    {
        Task<bool> BulkInsertAsync(List<Analytic> analyticsList);
    }
}
