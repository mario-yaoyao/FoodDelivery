using FoodDelivery.Protos;
using Grpc.Core;

namespace FoodDelivery.API.GrpcServices;

public class DispatchGrpcService : DispatchGrpc.DispatchGrpcBase
{
    public override async Task TrackDelivery(
        IAsyncStreamReader<RiderStatusRequest> requestStream,
        IServerStreamWriter<DispatcherInstructionResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            string instruction = request.Status switch
            {
                "Available" => "Order Assigned",
                "Accepted" => "Proceed To Restaurant",
                "ArrivedAtRestaurant" => "Wait For Order",
                "PickedUpOrder" => "Proceed To Customer",
                "Delivered" => "Delivery Completed",
                _ => "Unknown Status"
            };

            await responseStream.WriteAsync(
                new DispatcherInstructionResponse
                {
                    Instruction = instruction
                });
        }
    }
}