using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResellerService.Aplication.Abstractions.Data;
using ResellerService.Core.Interfaces;
using ResellerService.Infrastructure.Data;

namespace ResellerService.Infrastructure;

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
                .UseInMemoryDatabase("ResellerDatabase"));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}
