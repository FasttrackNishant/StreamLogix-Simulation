using NotificationService.API.Kafka;

namespace NotificationService.API.Interfaces;

public interface IShipmentNotifier
{
    Task NotifyShipmentStatusAsync(ShipmentStatusEvent shipmentEvent);
}