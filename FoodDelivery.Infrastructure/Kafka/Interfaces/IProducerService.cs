namespace FoodDelivery.Infrastructure.Kafka.Interfaces
{
    public interface IProducerService
    {
        Task PublishAsync<T>(
            string topic,
            T message,
            string? key = null,
            CancellationToken cancellationToken = default);
    }
}
