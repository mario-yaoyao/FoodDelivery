using FoodDelivery.Infrastructure.Kafka;
using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.PaymentConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddHostedService<PaymentWorker>();

var host = builder.Build();
host.Run();
