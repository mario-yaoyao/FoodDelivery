namespace FoodDelivery.Models.Models
{
    public class MenuItem : Base
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Restaurant Restaurant { get; set; } = null!;
    }
}
