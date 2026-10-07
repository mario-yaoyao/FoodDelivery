using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.NotificationConsumer;

public class NotificationWorker(IConsumerService<OrderCreatedEvent> consumerService, ILogger<NotificationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await consumerService.StartAsync(
               "order-created",
               "notification-group",
               async message =>
               {
                   logger.LogInformation(
                       "Notification received Order {OrderId}",
                       message.OrderId);

                   // Note: Implement notification logic here

                   await Task.CompletedTask;
               },
               stoppingToken);
    }
}
