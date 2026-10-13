using FoodDelivery.AnalyticsConsumer;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Data;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

builder.Services.AddHostedService<AnalyticsWorker>();

var host = builder.Build();
host.Run();


//builder.Services.AddSingleton<IProducerService, ProducerService>();

//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));

//builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IRiderRepository, RiderRepository>();

//builder.Services.AddHostedService<RiderWorker>();