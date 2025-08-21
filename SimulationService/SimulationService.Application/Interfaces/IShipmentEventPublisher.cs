using SimulationService.Domain.Events;

namespace SimulationService.Application.Interfaces;

public interface IShipmentEventPublisher
{
    Task PublishShipmentStatusUpdatedAsync(ShipmentStatusUpdatedEvent statusEvent, CancellationToken cancellationToken);
}