using FoodDelivery.AnalyticsConsumer;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddHostedService<AnalyticsWorker>();

var host = builder.Build();
host.Run();
