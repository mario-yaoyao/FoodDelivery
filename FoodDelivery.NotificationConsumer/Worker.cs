using FoodDelivery.Infrastructure.Email.Interfaces;
using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.NotificationConsumer;

public class NotificationWorker(
    IConsumerService<OrderCreatedEvent> orderCreatedConsumer,
    IConsumerService<RiderAssignedEvent> riderAssignedConsumer,
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var orderCreatedTask = orderCreatedConsumer.StartAsync(
            "order-created",
            "notification-group",
            async message =>
            {
                logger.LogInformation(
                    "Creating order for Order {OrderId}...",
                    message.OrderId);

                using var scope = scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                await emailService.SendEmailAsync(
                    message.CustomerName,
                    message.CustomerEmail,
                    "Order Created (Broadcast)",
                    $"Your order #{message.OrderId} on {message.RestaurantName} has been created.");
            },
            stoppingToken);

        var riderAssignedTask = riderAssignedConsumer.StartAsync(
            "rider-assigned",
            "notification-group",
            async message =>
            {
                logger.LogInformation(
                    "Assigning rider for Order {OrderId}...",
                    message.OrderId);

                using var scope = scopeFactory.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                await emailService.SendEmailAsync(
                    message.CustomerName,
                    message.CustomerEmail,
                    "Rider Assigned (Round Robin)",
                    message.Body
                );
            },
            stoppingToken);

        await Task.WhenAll(
            orderCreatedTask,
            riderAssignedTask);
    }
}
