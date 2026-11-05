using Confluent.Kafka;
using FoodDelivery.Infrastructure.Kafka.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoodDelivery.Infrastructure.Kafka
{
    public class ConsumerService<T>(
        IConfiguration configuration,
        ILogger<ConsumerService<T>> logger)
        : IConsumerService<T>
    {
        public async Task StartAsync(
            string topic,
            string groupId,
            Func<T, Task> handler,
            CancellationToken cancellationToken)
        {
            var bootstrapServers =
                configuration.GetConnectionString("KafkaServer");

            logger.LogInformation(
                "Starting Kafka consumer. Topic: {Topic}, Group: {GroupId}, BootstrapServers: {BootstrapServers}",
                topic,
                groupId,
                bootstrapServers);

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                SessionTimeoutMs = 6000,
                SocketKeepaliveEnable = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config)
                .SetErrorHandler((_, error) =>
                {
                    logger.LogError(
                        "Kafka error on topic {Topic}: {Reason}",
                        topic,
                        error.Reason);
                })
                .SetPartitionsAssignedHandler((_, partitions) =>
                {
                    logger.LogInformation(
                        "Partitions assigned for topic {Topic}: {Partitions}",
                        topic,
                        string.Join(", ", partitions));
                })
                .SetPartitionsRevokedHandler((_, partitions) =>
                {
                    logger.LogWarning(
                        "Partitions revoked for topic {Topic}: {Partitions}",
                        topic,
                        string.Join(", ", partitions));
                })
                .Build();

            try
            {
                consumer.Subscribe(topic);

                logger.LogInformation(
                    "Subscribed to topic '{Topic}' using group '{GroupId}'",
                    topic,
                    groupId);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        logger.LogDebug(
                            "Waiting for Kafka message on topic '{Topic}'",
                            topic);

                        var result = consumer.Consume(cancellationToken);

                        if (result?.Message?.Value is null)
                        {
                            continue;
                        }

                        logger.LogInformation(
                            "Received message from Topic={Topic}, Partition={Partition}, Offset={Offset}",
                            result.Topic,
                            result.Partition.Value,
                            result.Offset.Value);

                        var message =
                            JsonSerializer.Deserialize<T>(
                                result.Message.Value);

                        if (message is null)
                        {
                            logger.LogWarning(
                                "Failed to deserialize message from topic '{Topic}'",
                                topic);

                            continue;
                        }

                        await handler(message);

                        logger.LogInformation(
                            "Successfully processed message from topic '{Topic}'",
                            topic);
                    }
                    catch (ConsumeException ex)
                    {
                        logger.LogError(
                            ex,
                            "Kafka consume error on topic '{Topic}': {Reason}",
                            topic,
                            ex.Error.Reason);
                    }
                    catch (JsonException ex)
                    {
                        logger.LogError(
                            ex,
                            "Message deserialization error on topic '{Topic}'",
                            topic);
                    }
                    catch (OperationCanceledException)
                    {
                        logger.LogInformation(
                            "Consumer cancellation requested for topic '{Topic}'",
                            topic);

                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            ex,
                            "Unexpected consumer error on topic '{Topic}'",
                            topic);

                        await Task.Delay(
                            TimeSpan.FromSeconds(5),
                            cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(
                    ex,
                    "Fatal Kafka consumer startup failure for topic '{Topic}'",
                    topic);

                throw;
            }
            finally
            {
                logger.LogInformation(
                    "Closing consumer for topic '{Topic}'",
                    topic);

                consumer.Close();
            }
        }
    }
}