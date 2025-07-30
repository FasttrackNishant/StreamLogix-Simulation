using MediatR;
using ShipmentService.UseCases.Shipments.Dtos;

namespace ShipmentService.Application.UseCases.Shipments.GetById;

public class GetShipmentByIdQuery : IRequest<ShipmentDto>
{
    public int Id { get; set; }

    public GetShipmentByIdQuery(int id)
    {
        Id = id;
    }
}