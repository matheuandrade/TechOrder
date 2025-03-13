using CustomerOrderService.Application.Dto;
using CustomerOrderService.Application.Order.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrderService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddScoped<IValidator<OrderDto>, CreateOrderCustomerCommandValidator>();

        return services;
    }
}
