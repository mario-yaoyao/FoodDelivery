namespace FoodDelivery.BLL.Interfaces
{
    public interface IConsumerService<T>
    {
        Task StartAsync(
            string topic,
            string groupId,
            Func<T, Task> handler,
            CancellationToken cancellationToken);
    }
}
