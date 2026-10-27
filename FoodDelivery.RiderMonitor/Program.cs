using FoodDelivery.Protos;
using FoodDelivery.RiderMonitor;

var builder = Host.CreateApplicationBuilder(args);
var apiUrlString = builder.Configuration.GetConnectionString("ApiUrl");

builder.Services.AddHostedService<RiderMonitorWorker>();

builder.Services.AddGrpcClient<RiderGrpc.RiderGrpcClient>(
    options =>
    {
        options.Address = new Uri(apiUrlString!);
    });

var host = builder.Build();
host.Run();
