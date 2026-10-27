using Dapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class AnalyticsRepository(IDbConnectionFactory connectionFactory, ILogger<AnalyticsRepository> logger) : IAnalyticsRepository
    {
        public async Task<bool> BulkInsertAsync(IEnumerable<Analytic> analytics)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var insertQuery = @"
                    INSERT INTO Analytics
                    (
                        OrderId,
                        CustomerId,
                        RestaurantId,
                        RiderId,
                        TotalAmount,
                        Status,
                        OrderedAt,
                        DeliveredAt,
                        CreatedAt,
                        IsDeleted
                    )
                    VALUES
                    (
                        @OrderId,
                        @CustomerId,
                        @RestaurantId,
                        @RiderId,
                        @TotalAmount,
                        @Status,
                        @OrderedAt,
                        @DeliveredAt,
                        @CreatedAt,
                        @IsDeleted
                    );
                ";

                var rowsAffected = await connection.ExecuteAsync(insertQuery, analytics);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occurred while adding analytics records: {Message}", ex.Message);
                throw;
            }
        }
    }
}
