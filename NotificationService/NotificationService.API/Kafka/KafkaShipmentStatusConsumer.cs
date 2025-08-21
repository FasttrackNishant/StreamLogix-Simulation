using Confluent.Kafka;
using Newtonsoft.Json;
using NotificationService.API.Interfaces;

namespace NotificationService.API.Kafka;

public class KafkaShipmentStatusConsumer : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaShipmentStatusConsumer> _logger;

    public KafkaShipmentStatusConsumer(IServiceProvider serviceProvider, ILogger<KafkaShipmentStatusConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "notification-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🚀 Starting KafkaShipmentStatusConsumer...");
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛑 Stopping KafkaShipmentStatusConsumer...");
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🔄 KafkaShipmentStatusConsumer ExecuteAsync started");
        
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        
        try
        {
            consumer.Subscribe("shipment-status-updated");
            _logger.LogInformation("✅ Successfully subscribed to 'shipment-status-updated' topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(1));
                    
                    if (result == null)
                    {
                        // No message received within timeout, continue
                        continue;
                    }

                    if (result.IsPartitionEOF)
                    {
                        _logger.LogDebug("Reached end of partition");
                        continue;
                    }

                    _logger.LogInformation("📨 [NotificationService] Received message: {Message}", result.Message.Value);

                    var statusEvent = JsonConvert.DeserializeObject<ShipmentStatusEvent>(result.Message.Value);

                    if (statusEvent == null)
                    {
                        _logger.LogWarning("⚠️ Failed to deserialize message: {Message}", result.Message.Value);
                        consumer.Commit(result);
                        continue;
                    }

                    using var scope = _serviceProvider.CreateScope();
                    var notifier = scope.ServiceProvider.GetRequiredService<IShipmentNotifier>();
                    
                    await notifier.NotifyShipmentStatusAsync(statusEvent);
                    consumer.Commit(result);
                    
                    _logger.LogInformation("✅ Successfully processed shipment status for ID: {ShipmentId}", statusEvent.ShipmentId);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError("❌ Kafka consume error: {ErrorCode} - {ErrorReason}", ex.Error.Code, ex.Error.Reason);
                    
                    // Don't break the loop for consume exceptions
                    await Task.Delay(1000, stoppingToken);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "❌ JSON deserialization error");
                    // Skip this message and continue
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Unexpected error processing message");
                    
                    // Add delay to prevent tight loop on persistent errors
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("🛑 KafkaShipmentStatusConsumer operation was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "💥 Fatal error in KafkaShipmentStatusConsumer");
            throw;
        }
        finally
        {
            try
            {
                consumer.Close();
                _logger.LogInformation("🔒 KafkaShipmentStatusConsumer stopped and consumer closed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing Kafka consumer");
            }
        }
    }
}

public class ShipmentStatusEvent
{
    public int ShipmentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}
