using FluentValidation;
using MediatR;
using SharedKernel;
using SupplierOrderService.Application.Dto;
using SupplierOrderService.Application.Order.Create;
using SupplierOrderService.Extensions;
using SupplierOrderService.Infrastructure;

namespace SupplierOrderService.Api.Endpoints.Order;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (OrderDto request,
                                         IValidator<OrderDto> validator,
                                         ISender sender,
                                         CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var command = new CreateOrderCommand(request);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(id => Results.Created($"/orders/{id}", new { id }), CustomResults.Problem);
        })
        .WithTags("orders");
    }
}
