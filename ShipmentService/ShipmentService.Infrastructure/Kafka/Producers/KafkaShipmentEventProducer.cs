using Confluent.Kafka;
using ShipmentService.Application.Interfaces;
using ShipmentService.Infrastructure.Kafka.Messages;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ShipmentService.Infrastructure.Kafka.Producers;

public class KafkaShipmentEventProducer(IProducer<string, string> producer) : IShipmentEventPublisher
{
    private const string Topic = "shipment-triggered";

    public async Task PublishShipmentTriggeredAsync(object shipmentEvent, CancellationToken cancellationToken)
    {
       var jsonMessage = JsonConvert.SerializeObject(shipmentEvent);

        await producer.ProduceAsync(
            Topic,
            new Message<string, string> 
            { 
                Key = Guid.NewGuid().ToString(),
                Value = jsonMessage 
            },
            cancellationToken
        );
    }
}