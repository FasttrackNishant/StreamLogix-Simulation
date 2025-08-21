using MediatR;
using ShipmentService.Application.Interfaces;
using ShipmentService.Domain.Constants;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Application.UseCases.Shipments.Trigger;

public record TriggerShipmentCommand(int ShipmentId, int TriggeredByUserId) : IRequest<bool>;


public class TriggerShipmentCommandHandler : IRequestHandler<TriggerShipmentCommand, bool>
{
    private readonly IShipmentRepository _repository;

    private readonly IShipmentEventPublisher _shipmentEventPublisher;

    public TriggerShipmentCommandHandler(IShipmentRepository repository, IShipmentEventPublisher shipmentEventPublisher)
    {
        _repository = repository;
        _shipmentEventPublisher = shipmentEventPublisher;
    }

    public async Task<bool> Handle(TriggerShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.ShipmentId);

        if (shipment == null)
            throw new Exception("Shipment not found");

        if (shipment.ShipmentStatusId != ShipmentStatusConstants.Draft)
            throw new Exception("Only draft shipments can be triggered");

        shipment.ShipmentStatusId = ShipmentStatusConstants.Pending;
        shipment.TriggeredBy = request.TriggeredByUserId;
        shipment.TriggeredAt = DateTime.UtcNow;

        await _repository.UpdateAsync(shipment);

        var eventData = new
        {
            ShipmentId = request.ShipmentId,
            TriggeredBy = request.TriggeredByUserId,
            Timestamp = DateTime.UtcNow
        };

        // Publish event to Kafka
        await _shipmentEventPublisher.PublishShipmentTriggeredAsync(eventData, cancellationToken);

        return true;
    }
}