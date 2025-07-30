using Microsoft.EntityFrameworkCore;
using ShipmentService.Application.Interfaces;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Persistence.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly ShipmentDbContext _context;

    public ShipmentRepository(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();
        return shipment.Id;
    }

    public async Task<Shipment?> GetByIdAsync(int id)
    {
        return await _context.Shipments.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Shipment shipment)
    {
        _context.Shipments.Update(shipment);
        await _context.SaveChangesAsync();
    }
}