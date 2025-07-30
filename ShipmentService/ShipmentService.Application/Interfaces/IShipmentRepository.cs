using ShipmentService.Domain.Entities;

namespace ShipmentService.Application.Interfaces;

/// <summary>
/// This is Interface for the Shipment
/// </summary>
public interface IShipmentRepository
{
    Task<int> CreateAsync(Shipment shipment);
    Task<Shipment?> GetByIdAsync(int id);
    Task UpdateAsync(Shipment shipment);

}