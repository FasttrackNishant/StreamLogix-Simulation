using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SimulationService.Domain.Events;
using SimulationService.Application.Interfaces;

namespace SimulationService.Infrastructure.Kafka.Consumers;

public class KafkaShipmentEventConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IShipmentEventPublisher _publisher;

    public KafkaShipmentEventConsumer(IShipmentEventPublisher publisher)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "simulation-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe("shipment-triggered");

        _publisher = publisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var zones = new[] { "Entry", "Packing", "Transit", "Exit" };

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = _consumer.Consume(stoppingToken);

            var triggeredEvent = JsonConvert.DeserializeObject<ShipmentTriggeredEventMessage>(result.Message.Value);

            Console.WriteLine($"[SimulationService] Shipment {triggeredEvent.ShipmentId} triggered. Starting simulation...");

            foreach (var zone in zones)
            {
                var statusEvent = new ShipmentStatusUpdatedEvent
                {
                    ShipmentId = triggeredEvent.ShipmentId,
                    Zone = zone,
                    Status = "In Transit",
                    Timestamp = DateTime.UtcNow
                };

                await _publisher.PublishShipmentStatusUpdatedAsync(statusEvent, stoppingToken);

                Console.WriteLine($"[SimulationService] Shipment {statusEvent.ShipmentId} entered {zone}.");

                await Task.Delay(2000, stoppingToken); // simulate 2s delay
            }

            Console.WriteLine($"[SimulationService] Shipment {triggeredEvent.ShipmentId} simulation completed.");
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}

public class ShipmentTriggeredEventMessage
{
    public int ShipmentId { get; set; }
    public DateTime TriggeredAt { get; set; }
}