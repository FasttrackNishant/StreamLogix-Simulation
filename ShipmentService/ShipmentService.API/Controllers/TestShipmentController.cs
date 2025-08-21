using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShipmentService.Application.Interfaces;
using ShipmentService.Infrastructure.Kafka.Messages;

[ApiController]
[Route("api/[controller]")]
public class TestShipmentController : ControllerBase
{
    private readonly IShipmentEventPublisher _publisher;

    public TestShipmentController(IShipmentEventPublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendTestEvent()
    {
        var shipmentEvent = new ShipmentTriggeredEventMessage
        {
            ShipmentId = 101,
            TriggeredAt = DateTime.UtcNow
        };

        await _publisher.PublishShipmentTriggeredAsync(shipmentEvent, CancellationToken.None);
        return Ok("Event sent to Kafka");
    }
    
    [HttpPost("test-status")]
    public async Task<IActionResult> TestStatus([FromServices] IProducer<string, string> producer)
    {
        var testEvent = new
        {
            ShipmentId = 99,
            Status = "In Transit",
            UpdatedAt = DateTime.UtcNow
        };

        await producer.ProduceAsync(
            "shipment-status-updated",
            new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonConvert.SerializeObject(testEvent)
            });

        return Ok("Test message sent");
    }
}