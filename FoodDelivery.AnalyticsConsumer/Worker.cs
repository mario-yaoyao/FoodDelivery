using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.AnalyticsConsumer;

public class AnalyticsWorker(IConsumerService<OrderCreatedEvent> consumerService, ILogger<AnalyticsWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumerService.StartAsync(
               "order-created",
               "analytics-group",
               async message =>
               {
                   logger.LogInformation(
                       "Analytics received Order {OrderId}",
                       message.OrderId);

                   // Implement analytics logic here

                   await Task.CompletedTask;
               },
               stoppingToken);
    }
}
