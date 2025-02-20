// API/Controllers/MessageController.cs
using Core.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly RabbitMQService _rabbitMq;
    private const string QueueName = "message_queue";

    public MessageController(RabbitMQService rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    [HttpPost]
    public IActionResult SendMessage([FromBody] Message message)
    {
        message.Timestamp = DateTime.UtcNow;
        message.Status = "Sent";

        _rabbitMq.SendMessage(message, QueueName);
        return Ok(new { message = "Message sent successfully" });
    }

    [HttpGet("start-receiving")]
    public IActionResult StartReceiving()
    {
        _rabbitMq.StartReceiving<Message>(QueueName, message =>
        {
            Console.WriteLine($"Received message: {message.Content} at {message.Timestamp}");
        });

        return Ok(new { message = "Started receiving messages" });
    }
}