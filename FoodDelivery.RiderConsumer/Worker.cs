using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.RiderConsumer
{
    public class RiderWorker(IConsumerService<DeliveryRequestedEvent> consumerService, ILogger<RiderWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumerService.StartAsync(
                "delivery-requested",
                "rider-group",
                async message =>
                {
                    logger.LogInformation(
                        "Rider received Order {OrderId}",
                        message.OrderId);

                    // Implement rider logic here

                    await Task.CompletedTask;
                },
                stoppingToken);
        }
    }
}
