using FoodDelivery.Models.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IAnalyticsRepository
    {
        Task BulkInsertAsync(IEnumerable<Analytic> analytics);
    }
}
