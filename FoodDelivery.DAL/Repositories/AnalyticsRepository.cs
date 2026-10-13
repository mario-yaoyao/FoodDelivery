using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class AnalyticsRepository(AppDbContext context, ILogger<AnalyticsRepository> logger) : IAnalyticsRepository
    {
        public async Task BulkInsertAsync(IEnumerable<Analytic> analytics)
        {
            try
            {
                await context.Analytics.AddRangeAsync(analytics);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while retrieving menu item by ID: {Message}", ex.Message);
                throw;
            }
        }
    }
}
