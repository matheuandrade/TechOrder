﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SupplierOrderService.Application.Dto;
using SupplierOrderService.Application.Dtos;
using SupplierOrderService.Application.Order.Create;
using SupplierOrderService.Application.Reseller.Register;

namespace SupplierOrderService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddScoped<IValidator<ResellerDto>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<OrderDto>, CreateOrderCommandValidator>();

        return services;
    }
}
