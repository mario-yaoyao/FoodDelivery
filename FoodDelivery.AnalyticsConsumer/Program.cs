using FoodDelivery.AnalyticsConsumer;
using FoodDelivery.Infrastructure.Kafka;
using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.Protos;

var builder = Host.CreateApplicationBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");
var apiUrlString = builder.Configuration.GetConnectionString("ApiUrl");

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddHostedService<AnalyticsWorker>();

builder.Services.AddGrpcClient<AnalyticsGrpc.AnalyticsGrpcClient>(
    options =>
    {
        options.Address = new Uri(apiUrlString!);
    });

var host = builder.Build();
host.Run();
