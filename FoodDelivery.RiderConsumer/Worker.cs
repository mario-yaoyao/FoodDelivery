using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.RiderConsumer
{
    public class RiderWorker(
        IConsumerService<DeliveryRequestedEvent> consumerService,
        IProducerService producerService,
        IServiceScopeFactory scopeFactory,
        ILogger<RiderWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumerService.StartAsync(
                "delivery-requested",
                "rider-group",
                async message =>
                {
                    logger.LogInformation(
                        "Assigning rider for Order {OrderId}...",
                        message.OrderId);

                    using var scope = scopeFactory.CreateScope();
                    var riderRepository = scope.ServiceProvider.GetRequiredService<IRiderRepository>();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                    var rider = await riderRepository.GetAvailableRiderAsync();

                    if (rider is null)
                    {
                        logger.LogWarning(
                            "No available rider found for Order {OrderId}",
                            message.OrderId);

                        return;
                    }

                    var existingOrder = await orderRepository.GetOrderByIdAsync(message.OrderId); 

                    if (existingOrder is null)
                    {
                        logger.LogWarning(
                            "Order {OrderId} not found",
                            message.OrderId);
                        return;
                    }

                    existingOrder.RiderId = rider.Id;
                    await orderRepository.SaveChangesAsync();

                    // Event Chained Unicast - Notification
                    await producerService.PublishAsync<RiderAssignedEvent>(
                        "rider-assigned",
                        new RiderAssignedEvent
                        {
                            OrderId = message.OrderId,
                            CustomerName = message.CustomerName,
                            CustomerEmail = message.CustomerEmail,
                            RestaurantName = message.RestaurantName,
                            Body = $"Your order ORD-{message.OrderId} from {message.RestaurantName} has been assigned to rider {rider.FullName}."
                        });


                    logger.LogInformation(
                        "Assigned Rider {RiderId} to Order {OrderId}",
                        rider.Id,
                        message.OrderId);
                },
                stoppingToken);
        }
    }
}
