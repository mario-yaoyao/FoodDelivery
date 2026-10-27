using Dapper;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class MenuItemRepository(IDbConnectionFactory connectionFactory, ILogger<MenuItemRepository> logger) : IMenuItemRepository
    {
        public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
        {
            try
            {
                using var connection = connectionFactory.CreateConnection();

                var fetchQuery = @"
                    SELECT *
                    FROM MenuItems
                    WHERE Id = @MenuItemId;
                ";

                return await connection.QueryFirstOrDefaultAsync<MenuItem>(
                    fetchQuery,
                    new { MenuItemId = menuItemId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while retrieving menu item by ID: {Message}", ex.Message);
                throw;
            }
        }
    }
}