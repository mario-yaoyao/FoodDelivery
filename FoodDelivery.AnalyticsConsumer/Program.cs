using FoodDelivery.AnalyticsConsumer;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.Protos;
using Hangfire.DAL.Data;
using Hangfire.DAL.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");
var apiUrlString = builder.Configuration.GetConnectionString("ApiUrl");

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

builder.Services.AddHostedService<AnalyticsWorker>();

builder.Services.AddGrpcClient<AnalyticsGrpc.AnalyticsGrpcClient>(
    options =>
    {
        options.Address = new Uri(apiUrlString!);
    });

var host = builder.Build();
host.Run();
