using System;
using Microsoft.EntityFrameworkCore;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Persistence;

public class ShipmentDbContext : DbContext
{
    public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options) : base(options)
    {

    }
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(s => s.Id);
          
            entity.Property(s => s.ExpectedDelivery).IsRequired();

            entity.Property(s => s.TriggeredAt).IsRequired();

        });
    }
}
