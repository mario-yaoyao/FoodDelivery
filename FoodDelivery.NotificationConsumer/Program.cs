using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.NotificationConsumer;
using Hangfire.DAL.Data;
using Hangfire.DAL.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
