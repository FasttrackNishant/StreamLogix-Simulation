using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipmentService.Application.Interfaces;
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

        return services;
    }

}
