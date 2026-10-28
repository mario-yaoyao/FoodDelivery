using FoodDelivery.DispatchMonitor;
using FoodDelivery.Protos;

var builder = Host.CreateApplicationBuilder(args);
var apiUrlString = builder.Configuration.GetConnectionString("ApiUrl");

builder.Services.AddHostedService<DispatchMonitorWorker>();

builder.Services.AddGrpcClient<DispatchGrpc.DispatchGrpcClient>(
    options =>
    {
        options.Address = new Uri(apiUrlString!);
    });

var host = builder.Build();
host.Run();
