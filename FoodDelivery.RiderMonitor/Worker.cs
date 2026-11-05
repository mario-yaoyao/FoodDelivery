using FoodDelivery.Protos;
using Grpc.Core;

namespace FoodDelivery.RiderMonitor
{
    public class RiderMonitorWorker(
        RiderGrpc.RiderGrpcClient riderGrpcClient,
        ILogger<RiderMonitorWorker> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var call = riderGrpcClient.GetAvailableRiders(new EmptyRequest());

                    while (await call.ResponseStream.MoveNext(stoppingToken))
                    {
                        var rider = call.ResponseStream.Current;

                        logger.LogInformation(
                            "Available Rider: {Name}",
                            rider.FullName);
                    }

                    logger.LogInformation("All available riders displayed.");

                    break;
                }
                catch (RpcException ex)
                {
                    logger.LogWarning(
                        ex,
                        "Failed to get riders. Retrying in 5 seconds.");

                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
