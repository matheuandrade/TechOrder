using CustomerOrderService.Application.Abstractions.Data;
using CustomerOrderService.Core.Interfaces;
using CustomerOrderService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services) =>
        services
            .AddDatabase();

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseInMemoryDatabase("CustomerOrderDatabase"));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}
