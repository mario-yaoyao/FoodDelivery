namespace FoodDelivery.Models.DTOs.Requests
{
    public class CreateOrderItemReqDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
