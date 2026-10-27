using FoodDelivery.Protos;

namespace FoodDelivery.RiderMonitor
{
    public class RiderMonitorWorker(
        RiderGrpc.RiderGrpcClient riderGrpcClient,
        ILogger<RiderMonitorWorker> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
        }
    }
}
