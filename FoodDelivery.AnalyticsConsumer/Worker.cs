using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Models.Models;
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
                                OrderedAt = item.OrderedAt.ToString("O")
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

    private static Analytic CreateAnalytic(
        int orderId,
        int customerId,
        int restaurantId,
        decimal totalAmount,
        OrderStatus status,
        DateTime orderedAt,
        DateTime createdAt,
        bool isDeleted)
    {
        return new Analytic
        {
            OrderId = orderId,
            CustomerId = customerId,
            RestaurantId = restaurantId,
            TotalAmount = totalAmount,
            Status = status,
            OrderedAt = orderedAt,
            CreatedAt = createdAt,
            IsDeleted = isDeleted
        };
    }
}