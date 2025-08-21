namespace ShipmentService.Infrastructure.Kafka.Messages;

public class ShipmentTriggeredEventMessage
{
    public int ShipmentId { get; set; }
    public DateTime TriggeredAt { get; set; }
}