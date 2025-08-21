using Microsoft.AspNetCore.SignalR.Client;

namespace StreamLogix.UI.Services;

public class ShipmentHubClient
{
    private readonly HubConnection _connection;

    public event Action<string>? OnStatusReceived;

    public ShipmentHubClient()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5062/shipmentHub")
            .WithAutomaticReconnect()
            .Build();

        // Listen for events from NotificationService
        _connection.On<object>("ShipmentStatusUpdated", (data) =>
        {
            OnStatusReceived?.Invoke(data.ToString()!);
        });
    }

    public async Task StartAsync()
    {
        await _connection.StartAsync();
    }
}