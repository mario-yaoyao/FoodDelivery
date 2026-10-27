using FoodDelivery.BLL.Interfaces;
using FoodDelivery.Protos;
using Grpc.Core;

namespace FoodDelivery.API.GrpcServices;

public class OrderGrpcService(IOrderService orderService) : OrderGrpc.OrderGrpcBase
{
    public override async Task<OrderResponse> GetOrderById(GetOrderRequest request, ServerCallContext context)
    {
        var order = await orderService.GetOrderByIdAsync(request.OrderId);

        if (order is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
        }

        return new OrderResponse
        {
            OrderId = order.Id,
            CustomerId = order.UserId,
            CustomerName = order.User.FullName,
            CustomerEmail = order.User.Email
        };
    }

    public override async Task<AssignRiderResponse> AssignRiderToOrder(AssignRiderRequest request, ServerCallContext context)
    {
        var isSuccess = await orderService.UpdateRiderAssignmentAsync(
            request.OrderId,
            request.RiderId);

        return new AssignRiderResponse
        {
            Success = isSuccess
        };
    }
}