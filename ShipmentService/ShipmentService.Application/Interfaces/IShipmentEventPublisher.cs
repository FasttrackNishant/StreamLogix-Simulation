namespace ShipmentService.Application.Interfaces;

public interface IShipmentEventPublisher
{
 Task PublishShipmentTriggeredAsync(object shipmentEvent, CancellationToken cancellationToken);
}
