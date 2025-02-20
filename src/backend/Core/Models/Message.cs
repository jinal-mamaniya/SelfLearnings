namespace Core.Models;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Status { get; set; }
    public string? QueueName { get; set; }
}
