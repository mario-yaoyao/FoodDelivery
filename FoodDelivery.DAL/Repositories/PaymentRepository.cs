using Dapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class PaymentRepository(IDbConnectionFactory connectionFactory, ILogger<OrderRepository> logger) : IPaymentRepository
    {
        public async Task AddPaymentAsync(Payment payment)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var insertQuery = @"
                    INSERT INTO Payments
                    (
                        OrderId,
                        Amount,
                        Status,
                        Method,
                        CreatedAt,
                        IsDeleted
                    )
                    VALUES
                    (
                        @OrderId,
                        @Amount,
                        @Status,
                        @Method,
                        @CreatedAt,
                        @IsDeleted
                    );
                ";

                await connection.ExecuteAsync(insertQuery, payment);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occured while adding payment.");
                throw;
            }
        }
    }
}
