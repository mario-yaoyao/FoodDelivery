using Dapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class RiderRepository(IDbConnectionFactory connectionFactory, ILogger<RiderRepository> logger) : IRiderRepository
    {
        public async Task<Rider?> GetAvailableRiderAsync()
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var fetchQuery = @"
                    SELECT TOP 1 *
                    FROM Riders
                    WHERE Status = @Status;
                ";

                return await connection.QueryFirstOrDefaultAsync<Rider>(
                    fetchQuery,
                    new { Status = (int)RiderStatus.Available });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occurred while retrieving rider: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Rider>> GetAvailableRidersAsync()
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var fetchQuery = @"
                    SELECT *
                    FROM Riders
                    WHERE Status = @Status;
                ";

                return await connection.QueryAsync<Rider>(
                    fetchQuery,
                    new { Status = (int)RiderStatus.Available });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occurred while retrieving available riders: {Message}", ex.Message);
                throw;
            }
        }
    }
}
