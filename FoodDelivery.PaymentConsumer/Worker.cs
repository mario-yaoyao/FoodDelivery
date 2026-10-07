using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.PaymentConsumer
{
    public class PaymentWorker(IConsumerService<PaymentRequestedEvent> consumerService, ILogger<PaymentWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await consumerService.StartAsync(
                "payment-requested",
                "payment-group",
                async message =>
                {
                    logger.LogInformation(
                        "Processing payment for Order {OrderId}",
                        message.OrderId);

                    // Note: Implement payment logic here

                    await Task.CompletedTask;
                },
                stoppingToken);
        }
    }
}
// messageId (identifier) - implement a mechanism to control what partitions can receive kafka message