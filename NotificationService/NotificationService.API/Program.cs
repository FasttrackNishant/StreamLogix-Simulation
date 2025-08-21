using NotificationService.API.Hubs;
using NotificationService.API.Interfaces;
using NotificationService.API.Kafka;
using NotificationService.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Services
builder.Services.AddControllers();

builder.WebHost.UseUrls("http://0.0.0.0:5062", "https://0.0.0.0:7038");

Console.WriteLine("🔧 Configuring services...");

// Configure CORS before adding SignalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:8080") // Add your frontend URLs
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Important for SignalR
    });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true; // For development
});

builder.Services.AddScoped<IShipmentNotifier, SignalRShipmentNotifier>();

// This will break exposing
// Kafka background consumer
// builder.Services.AddHostedService<KafkaShipmentStatusConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable CORS
app.UseCors("SignalRPolicy");

// Add routing
app.UseRouting();

// Endpoints
app.MapControllers(); 
app.MapHub<ShipmentHub>("/shipmentHub");

app.MapGet("/", () => "Notification Service Running 🚀");

// Add a health check endpoint
app.MapGet("/health", () => new { 
    Status = "Healthy", 
    Timestamp = DateTime.UtcNow,
    SignalRHub = "/shipmentHub"
});

// Add an endpoint to test SignalR manually
app.MapPost("/test-notification", async (IServiceProvider serviceProvider) =>
{
    using var scope = serviceProvider.CreateScope();
    var notifier = scope.ServiceProvider.GetRequiredService<IShipmentNotifier>();
    
    var testEvent = new ShipmentStatusEvent
    {
        ShipmentId = 999,
        Status = "Test Status",
        UpdatedAt = DateTime.UtcNow
    };
    
    await notifier.NotifyShipmentStatusAsync(testEvent);
    return Results.Ok(new { Message = "Test notification sent", Event = testEvent });
});

Console.WriteLine("🚀 Notification Service started!");
Console.WriteLine("   SignalR Hub: /shipmentHub");
Console.WriteLine("   Health Check: /health");
Console.WriteLine("   Test Notification: /test-notification");

app.Run();