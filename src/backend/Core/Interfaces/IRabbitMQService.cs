namespace Core.Interfaces;

public interface IRabbitMQService
{
    void PublishMessage<T>(T message, string queueName);
    void SubscribeToQueue<T>(string queueName, Action<T> onMessageReceived);
}