using Confluent.Kafka;
using SimulationService.Application.Interfaces;
using SimulationService.Infrastructure.Kafka.Consumers;
using SimulationService.Infrastructure.Kafka.Producers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHostedService<KafkaShipmentEventConsumer>();

builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    return new ProducerBuilder<string, string>(config).Build();
});

builder.Services.AddSingleton<IShipmentEventPublisher, KafkaShipmentEventProducer>();
builder.Services.AddHostedService<KafkaShipmentEventConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "SimulationService running...");
app.Run();