using Confluent.Kafka;
using Newtonsoft.Json;
using SimulationService.Application.Interfaces;
using SimulationService.Domain.Events;

namespace SimulationService.Infrastructure.Kafka.Producers;

public class KafkaShipmentEventProducer(IProducer<string, string> producer) : IShipmentEventPublisher
{
    private const string Topic = "shipment-status-updated";

    public async Task PublishShipmentStatusUpdatedAsync(ShipmentStatusUpdatedEvent statusEvent, CancellationToken cancellationToken)
    {
        var jsonMessage = JsonConvert.SerializeObject(statusEvent);

        await producer.ProduceAsync(
            Topic,
            new Message<string, string>
            {
                Key = statusEvent.ShipmentId.ToString(),
                Value = jsonMessage
            },
            cancellationToken
        );
    }
}