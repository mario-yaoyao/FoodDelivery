using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DTOs.Requests
{
    public class CreateOrderReqDto
    {
        public int RestaurantId { get; set; }

        public List<CreateOrderItemReqDto> Items { get; set; } = [];
    }
}
