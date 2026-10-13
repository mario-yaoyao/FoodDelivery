using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.DTOs.Requests;
using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.BLL.Services
{
    public class OrderService(
        IOrderRepository orderRepository,
        IMenuItemRepository menuItemRepository,
        IPaymentRepository paymentRepository,
        IProducerService producerService
        ) : IOrderService
    {
        public async Task<bool> CreateOrderAsync(int userId, CreateOrderReqDto request)
        {
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var menuItem = await menuItemRepository.GetMenuItemByIdAsync(item.MenuItemId);

                if (menuItem is null) return false;
                if (menuItem.RestaurantId != request.RestaurantId) return false;

                orderItems.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = item.Quantity,
                    Price = menuItem.Price
                });

                totalAmount += menuItem.Price * item.Quantity;
            }

            var order = new Order
            {
                UserId = userId,
                RestaurantId = request.RestaurantId,
                TotalAmount = totalAmount,
                Status = OrderStatus.Created,
                OrderItems = orderItems
            };

            await orderRepository.AddOrderAsync(order);

            var createdOrder = await orderRepository.GetOrderByIdAsync(order.Id);
            if (createdOrder is null) return false;

             //Broadcast Implementation - Notification, Analytics
            await producerService.PublishAsync(
                "order-created",
                new OrderCreatedEvent
                {
                    OrderId = createdOrder.Id,
                    CustomerId = createdOrder.UserId,
                    CustomerName = createdOrder.User.FullName,
                    CustomerEmail = createdOrder.User.Email,
                    RestaurantId = createdOrder.RestaurantId,
                    RestaurantName = createdOrder.Restaurant.Name,
                    RiderId = createdOrder.RiderId,
                    TotalAmount = createdOrder.TotalAmount,
                    Status = createdOrder.Status,
                    CreatedAt = DateTime.UtcNow
                }
            );

            return true;
        }

        public async Task<bool> MarkOrderReadyAsync(int orderId)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);

            if (order is null) return false;

            order.Status = OrderStatus.ReadyForPickup;

            await orderRepository.SaveChangesAsync();

            // Round Robin Implementation - Rider
            await producerService.PublishAsync(
                "delivery-requested",
                new DeliveryRequestedEvent
                {
                    OrderId = order.Id,
                    CustomerName = order.User.FullName,
                    CustomerEmail = order.User.Email,
                    RestaurantName = order.Restaurant.Name,
                    RequestedAt = DateTime.UtcNow
                },
                $"ORD-{order.Id}"
                );

            return true;
        }

        public async Task<bool> ProcessPaymentAsync(int orderId)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);

            if (order is null) return false;

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Status = PaymentStatus.Pending,
                Method = PaymentMethod.CashOnDelivery
            };

            await paymentRepository.AddPaymentAsync(payment);

            // Unicast Implementation - Payment
            await producerService.PublishAsync(
                "payment-requested",
                new PaymentRequestedEvent
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    Amount = order.TotalAmount,
                    RequestedAt = DateTime.UtcNow
                });

            return true;
        }
    }
}