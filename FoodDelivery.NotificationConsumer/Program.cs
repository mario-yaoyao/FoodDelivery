using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.NotificationConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
