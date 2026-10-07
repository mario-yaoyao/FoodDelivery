using FoodDelivery.Models.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
    }
}
