using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.RiderConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddHostedService<RiderWorker>();

var host = builder.Build();
host.Run();
