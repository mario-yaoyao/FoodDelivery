using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class RiderRepository(AppDbContext context, ILogger<RiderRepository> logger) : IRiderRepository
    {
        public async Task<Rider?> GetAvailableRiderAsync()
        {
            try
            {
                return await context.Riders
                    .Where(r => r.Status == RiderStatus.Available)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occurred while retrieving rider: {Message}", ex.Message);
                throw;
            }
        }
    }
}
