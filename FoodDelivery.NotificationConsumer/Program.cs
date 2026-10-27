using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.Infrastructure.Email;
using FoodDelivery.Infrastructure.Email.Interfaces;
using FoodDelivery.Infrastructure.Kafka;
using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.NotificationConsumer;

var builder = Host.CreateApplicationBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
