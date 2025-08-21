using Microsoft.AspNetCore.SignalR;
using NotificationService.API.Hubs;
using NotificationService.API.Interfaces;
using NotificationService.API.Kafka;

namespace NotificationService.API.Services;

public class SignalRShipmentNotifier : IShipmentNotifier
{
    private readonly IHubContext<ShipmentHub> _hubContext;
    private readonly ILogger<SignalRShipmentNotifier> _logger;

    public SignalRShipmentNotifier(IHubContext<ShipmentHub> hubContext, ILogger<SignalRShipmentNotifier> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyShipmentStatusAsync(ShipmentStatusEvent shipmentEvent)
    {
        try
        {
            _logger.LogInformation("Sending SignalR notification for shipment {ShipmentId} with status {Status}", 
                shipmentEvent.ShipmentId, shipmentEvent.Status);
                
            await _hubContext.Clients.All.SendAsync("ShipmentStatusUpdated", shipmentEvent);
            
            _logger.LogInformation("Successfully sent SignalR notification for shipment {ShipmentId}", 
                shipmentEvent.ShipmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SignalR notification for shipment {ShipmentId}", 
                shipmentEvent.ShipmentId);
            throw;
        }
    }
}