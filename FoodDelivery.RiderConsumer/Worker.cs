using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.Protos;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.RiderConsumer
{
    public class RiderWorker(
        IConsumerService<DeliveryRequestedEvent> consumerService,
        IProducerService producerService,
        OrderGrpc.OrderGrpcClient orderGrpcClient,
        RiderGrpc.RiderGrpcClient riderGrpcClient,
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

                    var rider = await riderGrpcClient.GetAvailableRiderAsync(new EmptyRequest());

                    if (rider is null)
                    {
                        logger.LogWarning(
                            "No available rider found for Order {OrderId}",
                            message.OrderId);

                        return;
                    }

                    var existingOrder = await orderGrpcClient.GetOrderByIdAsync(
                        new GetOrderRequest
                        {
                            OrderId = message.OrderId
                        });

                    if (existingOrder is null)
                    {
                        logger.LogWarning(
                            "Order {OrderId} not found",
                            message.OrderId);
                        return;
                    }

                    await orderGrpcClient.AssignRiderToOrderAsync(
                        new AssignRiderRequest
                        {
                            OrderId = existingOrder.OrderId,
                            RiderId = rider.RiderId
                        });

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
                        rider.RiderId,
                        message.OrderId);
                },
                stoppingToken);
        }
    }
}
