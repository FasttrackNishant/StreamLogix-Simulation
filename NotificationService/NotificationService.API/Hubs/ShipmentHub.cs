using Microsoft.AspNetCore.SignalR;

namespace NotificationService.API.Hubs;

public class ShipmentHub : Hub
{
    private readonly ILogger<ShipmentHub> _logger;

    public ShipmentHub(ILogger<ShipmentHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}