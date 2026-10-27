using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.Protos;
using FoodDelivery.Shared.Contracts.Enums;
using FoodDelivery.Shared.Contracts.Events;

namespace FoodDelivery.AnalyticsConsumer;

public class AnalyticsWorker(
    IConsumerService<OrderCreatedEvent> consumerService,
    AnalyticsGrpc.AnalyticsGrpcClient analyticsGrpcClient,
    ILogger<AnalyticsWorker> logger
    ) : BackgroundService
{
    private readonly List<AnalyticsRequest> analyticsList = [];
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
                    (double)message.TotalAmount,
                    message.Status,
                    message.CreatedAt,
                    message.CreatedAt,
                    message.IsDeleted);

                analyticsList.Add(analytic);

                if (analyticsList.Count >= BatchSize)
                {
                    using var call = analyticsGrpcClient.BulkInsert();

                    foreach (var item in analyticsList)
                    {
                        await call.RequestStream.WriteAsync(
                            new AnalyticsRequest
                            {
                                OrderId = item.OrderId,
                                CustomerId = item.CustomerId,
                                RestaurantId = item.RestaurantId,
                                TotalAmount = (double)item.TotalAmount,
                                Status = item.Status.ToString(),
                                OrderedAt = item.OrderedAt
                            });
                    }

                    await call.RequestStream.CompleteAsync();

                    var response = await call.ResponseAsync;

                    logger.LogInformation(
                        "Inserted {Count} analytics records",
                        response.InsertedCount);

                    analyticsList.Clear();
                }
            },
            stoppingToken);
    }

    private static AnalyticsRequest CreateAnalytic(
        int orderId,
        int customerId,
        int restaurantId,
        double totalAmount,
        OrderStatus status,
        DateTime orderedAt,
        DateTime createdAt,
        bool isDeleted)
    {
        return new AnalyticsRequest
        {
            OrderId = orderId,
            CustomerId = customerId,
            RestaurantId = restaurantId,
            TotalAmount = totalAmount,
            Status = status.ToString(),
            OrderedAt = orderedAt.ToString("O"),
            CreatedAt = createdAt.ToString("O"),
            IsDeleted = isDeleted
        };
    }
}