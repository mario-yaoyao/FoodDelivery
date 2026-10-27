using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;

namespace FoodDelivery.BLL.Services
{
    public class RiderService(IRiderRepository riderRepository) : IRiderService
    {
        public async Task<Rider?> GetAvailableRiderAsync()
        {
            return await riderRepository.GetAvailableRiderAsync();
        }

        public async Task<IEnumerable<Rider>> GetAvailableRidersAsync()
        {
            return await riderRepository.GetAvailableRidersAsync();
        }
    }
}
