using FoodDelivery.Protos;
using Grpc.Core;

namespace FoodDelivery.DispatchMonitor;

public class DispatchMonitorWorker(
    DispatchGrpc.DispatchGrpcClient dispatchGrpcClient,
    ILogger<DispatchMonitorWorker> logger
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var call = dispatchGrpcClient.TrackDelivery();

        var sendTask = SendStatuses(call, logger, stoppingToken);
        var receiveTask = ReceiveInstructions(call, logger, stoppingToken);

        await Task.WhenAll(sendTask, receiveTask);
    }

    private static async Task SendStatuses(
        AsyncDuplexStreamingCall<RiderStatusRequest, DispatcherInstructionResponse> call,
        ILogger logger,
        CancellationToken token)
    {
        var statuses = new[]
        {
            "Available",
            "Accepted",
            "ArrivedAtRestaurant",
            "PickedUpOrder",
            "Delivered"
        };

        foreach (var status in statuses)
        {
            await call.RequestStream.WriteAsync(
                new RiderStatusRequest
                {
                    RiderId = 5,
                    Status = status
                });

            logger.LogInformation(
                "CLIENT -> Rider {RiderId}: {Status}",
                5,
                status);

            await Task.Delay(5000, token);
        }

        await call.RequestStream.CompleteAsync();
    }

    private static async Task ReceiveInstructions(
        AsyncDuplexStreamingCall<
            RiderStatusRequest,
            DispatcherInstructionResponse> call,
        ILogger logger,
        CancellationToken token)
    {
        await foreach (var response in call.ResponseStream.ReadAllAsync(token))
        {
            logger.LogInformation(
                "SERVER -> {Instruction}",
                response.Instruction);
        }
    }
}