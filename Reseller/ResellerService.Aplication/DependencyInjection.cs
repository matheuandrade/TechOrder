using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ResellerService.Aplication.Dtos;
using ResellerService.Aplication.Reseller.Register;

namespace ResellerService.Aplication;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddScoped<IValidator<ResellerDto>, RegisterUserCommandValidator>();

        return services;
    }
}
