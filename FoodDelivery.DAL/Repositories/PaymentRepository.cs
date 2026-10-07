using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class PaymentRepository(AppDbContext context, ILogger<OrderRepository> logger) : IPaymentRepository
    {
        public async Task AddPaymentAsync(Payment payment)
        {
            try
            {
                await context.Payments.AddAsync(payment);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while adding payment.");
                throw;
            }
        }
    }
}
