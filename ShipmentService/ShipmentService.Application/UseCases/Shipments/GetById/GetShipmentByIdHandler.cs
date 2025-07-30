using MediatR;
using ShipmentService.Application.Interfaces;
using ShipmentService.UseCases.Shipments.Dtos;

namespace ShipmentService.Application.UseCases.Shipments.GetById;

public class GetShipmentByIdHandler : IRequestHandler<GetShipmentByIdQuery, ShipmentDto>
{
    private readonly IShipmentRepository _shipmentRepository;

    public GetShipmentByIdHandler(IShipmentRepository shipmentRepository)
    {
        _shipmentRepository = shipmentRepository;
    }

    public async Task<ShipmentDto> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(request.Id);

        if (shipment is null)
        {
            throw new Exception("Shipment not found");
        }

        return new ShipmentDto
        {
            Id = shipment.Id,
            AssetId = shipment.AssetId,
            ShipmentStatusId = shipment.ShipmentStatusId,
            TriggeredAt = shipment.TriggeredAt,
            ExpectedDelivery = shipment.ExpectedDelivery,
            ExternalOrderId = shipment.ExternalOrderId
        };
    }
}