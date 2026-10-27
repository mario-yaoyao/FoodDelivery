using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;

namespace FoodDelivery.BLL.Services
{
    public class AnalyticsService(IAnalyticsRepository analyticsRepository) : IAnalyticsService
    {
        public async Task<bool> BulkInsertAsync(List<Analytic> analyticsList)
        {
            return await analyticsRepository.BulkInsertAsync(analyticsList);
        }
    }
}
