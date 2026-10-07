using Confluent.Kafka;
using FoodDelivery.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace FoodDelivery.BLL.Services
{
    public class ConsumerService<T>(IConfiguration configuration) : IConsumerService<T>
    {
        public async Task StartAsync(
            string topic,
            string groupId,
            Func<T, Task> handler,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Kafka Server: {configuration.GetConnectionString("KafkaServer")}");

            var config = new ConsumerConfig
            {
                //BootstrapServers = "localhost:9092", // Note: get value from appsettings.json
                BootstrapServers = configuration.GetConnectionString("KafkaServer"), // Note: get value from appsettings.json
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(cancellationToken);

                        if (result?.Message?.Value is null) continue;

                        var message = JsonSerializer.Deserialize<T>(result.Message.Value);

                        if (message is null) continue;

                        await handler(message);

                        Console.WriteLine($"Processed message from topic '{topic}'");
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Deserialization error: {ex.Message}");
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Kafka consume error: {ex.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Consumer for topic '{topic}' stopped.");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}