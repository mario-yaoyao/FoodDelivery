using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class OrderRepository(AppDbContext context, ILogger<OrderRepository> logger) : IOrderRepository
    {
        public async Task AddOrderAsync(Order order)
        {
            try
            {
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
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
                return await context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Restaurant)
                    .Include(o => o.Rider)
                    .FirstOrDefaultAsync(o => o.Id == orderId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while fetching order.");
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error occurred while saving order changes: {Message}", ex.Message);
                throw;
            }
        }
    }
}