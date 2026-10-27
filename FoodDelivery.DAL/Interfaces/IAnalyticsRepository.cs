using FoodDelivery.Models.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IAnalyticsRepository
    {
        Task<bool> BulkInsertAsync(IEnumerable<Analytic> analytics);
    }
}
