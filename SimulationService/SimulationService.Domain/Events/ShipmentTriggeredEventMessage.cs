namespace SimulationService.Domain.Events;

public class ShipmentTriggeredEventMessage
{
    public int ShipmentId { get; set; }
    public DateTime TriggeredAt { get; set; }
}