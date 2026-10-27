using FoodDelivery.Models.Models;

namespace FoodDelivery.BLL.Interfaces
{
    public interface IRiderService
    {
        Task<Rider?> GetAvailableRiderAsync();
        Task<IEnumerable<Rider>> GetAvailableRidersAsync();
    }
}
