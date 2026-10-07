using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.DAL.Repositories
{
    public class MenuItemRepository(AppDbContext context, ILogger<MenuItemRepository> logger) : IMenuItemRepository
    {
        public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
        {
            try
            {
                return await context.MenuItems
                    .FirstOrDefaultAsync(mi => mi.Id == menuItemId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while retrieving menu item by ID: {Message}", ex.Message);
                throw;
            }
        }
    }
}