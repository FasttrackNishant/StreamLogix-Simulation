using System;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipmentService.Application.Interfaces;
using ShipmentService.Infrastructure.Kafka.Producers;
using ShipmentService.Infrastructure.Persistence;
using ShipmentService.Infrastructure.Persistence.Repositories;

namespace ShipmentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShipmentDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("StreamLogixConnection"))
        );

        services.AddScoped<IShipmentRepository, ShipmentRepository>();

         services.AddSingleton<IProducer<string, string>>(sp =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };
                return new ProducerBuilder<string, string>(config).Build();
            });

            services.AddScoped<IShipmentEventPublisher, KafkaShipmentEventProducer>();

        return services;
    }

}
