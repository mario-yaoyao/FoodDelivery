using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using FoodDelivery.Shared.Contracts.Enums;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.AnalyticsConsumer;

public class AnalyticsWorker(
    IConsumerService<OrderCreatedEvent> consumerService,
    IServiceScopeFactory scopeFactory, 
    ILogger<AnalyticsWorker> logger) : BackgroundService
{
    private readonly List<Analytic> analyticsList = [];
    private const int BatchSize = 10;
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

                   var analytic = CreateAnalytic(
                        message.OrderId,
                        message.CustomerId,
                        message.RestaurantId,
                        message.TotalAmount,
                        message.Status,
                        message.CreatedAt);

                   analyticsList.Add(analytic);

                   if (analyticsList.Count >= BatchSize)
                   {
                       using var scope = scopeFactory.CreateScope();
                       var analyticRepository = scope.ServiceProvider.GetRequiredService<IAnalyticsRepository>();

                       await analyticRepository.BulkInsertAsync(analyticsList);

                       logger.LogInformation(
                           "Inserted {Count} analytics records",
                           analyticsList.Count);

                       analyticsList.Clear();
                   }

                   await Task.CompletedTask;
               },
               stoppingToken);
    }

    private static Analytic CreateAnalytic(
        int orderId,
        int customerId,
        int restaurantId,
        decimal totalAmount,
        OrderStatus status,
        DateTime orderedAt
        )
    {
        return new Analytic
        {
            OrderId = orderId,
            CustomerId = customerId,
            RestaurantId = restaurantId,
            TotalAmount = totalAmount,
            Status = status,
            OrderedAt = orderedAt
        };
    }
}
