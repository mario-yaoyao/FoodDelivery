using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Models.Models;
using FoodDelivery.Protos;
using FoodDelivery.Shared.Contracts.Enums;
using Grpc.Core;

namespace FoodDelivery.API.GrpcServices;

public class AnalyticsGrpcService(
    IAnalyticsRepository analyticsRepository
    ) : AnalyticsGrpc.AnalyticsGrpcBase
{
    public override async Task<BulkAnalyticsResponse> BulkInsert(
        IAsyncStreamReader<AnalyticsRequest> requestStream,
        ServerCallContext context)
    {
        var analytics = new List<Analytic>();

        await foreach (var request in requestStream.ReadAllAsync())
        {
            analytics.Add(new Analytic
            {
                OrderId = request.OrderId,
                CustomerId = request.CustomerId,
                RestaurantId = request.RestaurantId,
                TotalAmount = (decimal)request.TotalAmount,
                Status = Enum.Parse<OrderStatus>(request.Status),
                OrderedAt = DateTime.Parse(request.OrderedAt),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            });
        }

        await analyticsRepository.BulkInsertAsync(analytics);

        return new BulkAnalyticsResponse
        {
            Success = true,
            InsertedCount = analytics.Count
        };
    }
}