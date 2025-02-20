// Infrastructure/Services/RabbitMQService.cs
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using Infrastructure.Settings;

namespace Infrastructure.Services;

public class RabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(IOptions<RabbitMQSettings> options, ILogger<RabbitMQService> logger)
    {
        _logger = logger;

        _factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            Port = options.Value.Port,
            UserName = options.Value.UserName,
            Password = options.Value.Password
        };
    }

    public void SendMessage<T>(T message, string queueName)
    {
        try
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation("Message sent to queue: {QueueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to queue: {QueueName}", queueName);
            throw;
        }
    }

    public void StartReceiving<T>(string queueName, Action<T> messageHandler)
    {
        try
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<T>(json);

                    messageHandler(message);
                    _logger.LogInformation("Message processed from queue: {QueueName}", queueName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue: {QueueName}", queueName);
                }
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            _logger.LogInformation("Started receiving messages from queue: {QueueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting receiver for queue: {QueueName}", queueName);
            throw;
        }
    }
}