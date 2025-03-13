using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupplierOrderService.Application.Abstractions.Data;
using SupplierOrderService.Core.Interfaces;
using SupplierOrderService.Infrastructure.Data;

namespace SupplierOrderService.Infrastructure;

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
                .UseInMemoryDatabase("SupplierDatabase"));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}
