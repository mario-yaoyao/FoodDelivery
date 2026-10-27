using FoodDelivery.Models.Models;

namespace FoodDelivery.DAL.Interfaces
{
    public interface IRiderRepository
    {
        Task<Rider?> GetAvailableRiderAsync();
        Task<IEnumerable<Rider>> GetAvailableRidersAsync();
    }
}
