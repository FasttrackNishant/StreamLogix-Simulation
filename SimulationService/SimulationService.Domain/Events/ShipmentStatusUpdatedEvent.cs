namespace SimulationService.Domain.Events;

public class ShipmentStatusUpdatedEvent
{
    public int ShipmentId { get; set; }
    public string Zone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}