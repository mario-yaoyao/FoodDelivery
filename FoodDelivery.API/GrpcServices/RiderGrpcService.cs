using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.Protos;
using Grpc.Core;

namespace FoodDelivery.API.GrpcServices;

public class RiderGrpcService(IRiderRepository riderRepository) : RiderGrpc.RiderGrpcBase
{
    public override async Task<RiderResponse> GetAvailableRider(EmptyRequest request, ServerCallContext context)
    {
        var rider = await riderRepository.GetAvailableRiderAsync();

        if (rider is null)
        {
            throw new RpcException(
                new Status(
                    StatusCode.NotFound,
                    "No available rider found"));
        }

        return new RiderResponse
        {
            RiderId = rider.Id,
            FullName = rider.FullName
        };
    }

    public override async Task GetAvailableRiders(
        EmptyRequest request,
        IServerStreamWriter<RiderResponse> responseStream,
        ServerCallContext context)
    {
        var riders = await riderRepository.GetAvailableRidersAsync();

        foreach (var rider in riders)
        {
            await responseStream.WriteAsync(
                new RiderResponse
                {
                    RiderId = rider.Id,
                    FullName = rider.FullName
                });

            await Task.Delay(1000);
        }
    }
}
