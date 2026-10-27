using Dapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class OrderRepository(IDbConnectionFactory connectionFactory, ILogger<OrderRepository> logger) : IOrderRepository
    {
        public async Task AddOrderAsync(Order order)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var insertQuery = @"
                    INSERT INTO Orders
                    (
                        UserId,
                        RiderId,
                        RestaurantId,
                        TotalAmount,
                        Status,
                        CreatedAt,
                        IsDeleted
                    )
                        VALUES
                        (
                            @UserId,
                            @RiderId,
                            @RestaurantId,
                            @TotalAmount,
                            @Status,
                            @CreatedAt,
                            @IsDeleted
                        );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                order.Id = await connection.QuerySingleAsync<int>(
                    insertQuery,
                    order
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while adding order.");
                throw;
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var fetchQuery = @"
                    SELECT
                        o.*,
                        u.*,
                        r.*,
                        rd.*
                    FROM Orders o
                    INNER JOIN Users u ON o.UserId = u.Id
                    INNER JOIN Restaurants r ON o.RestaurantId = r.Id
                    LEFT JOIN Riders rd ON o.RiderId = rd.Id
                    WHERE o.Id = @OrderId;
                    ";

                var order = await connection.QueryAsync<Order, User, Restaurant, Rider, Order>(
                    fetchQuery,
                    (order, user, restaurant, rider) =>
                    {
                        order.User = user;
                        order.Restaurant = restaurant;
                        order.Rider = rider;
                        return order;
                    },
                    new { OrderId = orderId },
                    splitOn: "Id,Id,Id"
                );

                return order.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occured while fetching order.");
                throw;
            }
        }

        public async Task UpdateOrderStatusAsync(
            int orderId,
            OrderStatus status)
        {
            using var connection = connectionFactory.CreateConnection();

            var updateQuery = @"
                UPDATE Orders
                SET Status = @Status
                WHERE Id = @OrderId;
            ";

            await connection.ExecuteAsync(
                updateQuery,
                new
                {
                    OrderId = orderId,
                    Status = status
                });
        }

        public async Task<bool> UpdateRiderAssignmentAsync(int orderId, int riderId)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var updateQuery = @"
                    UPDATE Orders
                    SET RiderId = @RiderId
                    WHERE Id = @OrderId;
                ";

                var rowsAffected = await connection.ExecuteAsync(
                    updateQuery,
                    new
                    {
                        OrderId = orderId,
                        RiderId = riderId
                    });

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Database error while assigning rider to order.");

                throw;
            }
        }
    }
}