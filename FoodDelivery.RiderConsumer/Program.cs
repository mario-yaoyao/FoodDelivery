using Confluent.Kafka;
using FoodDelivery.Infrastructure.Kafka;
using FoodDelivery.Infrastructure.Kafka.Interfaces;
using FoodDelivery.Protos;
using FoodDelivery.RiderConsumer;

var builder = Host.CreateApplicationBuilder(args);
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
builder.Services.AddSingleton(typeof(IConsumerService<>), typeof(ConsumerService<>));

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
