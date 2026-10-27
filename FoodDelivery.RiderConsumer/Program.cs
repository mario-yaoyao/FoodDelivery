using Confluent.Kafka;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.BLL.Services;
using FoodDelivery.DAL.Interfaces;
using FoodDelivery.DAL.Repositories;
using FoodDelivery.Protos;
using FoodDelivery.RiderConsumer;
using Hangfire.DAL.Data;
using Hangfire.DAL.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("FoodDeliveryDatabase");
var kafkaConnectionString = builder.Configuration.GetConnectionString("KafkaServer");
var apiUrlString = builder.Configuration.GetConnectionString("ApiUrl");

var producerConfig = new ProducerConfig
{
    BootstrapServers = kafkaConnectionString,
    AllowAutoCreateTopics = true,
    Acks = Acks.All
};

builder.Services.AddSingleton<IProducer<string, string>>(
    _ => new ProducerBuilder<string, string>(producerConfig).Build());

builder.Services.AddSingleton<IProducerService, ProducerService>();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRiderRepository, RiderRepository>();

builder.Services.AddHostedService<RiderWorker>();

builder.Services.AddGrpcClient<OrderGrpc.OrderGrpcClient>(
    options =>
    {
        options.Address = new Uri(apiUrlString!);
    });

builder.Services.AddGrpcClient<RiderGrpc.RiderGrpcClient>(
    options =>
    {
        options.Address = new Uri(apiUrlString!);
    });

var host = builder.Build();
host.Run();
