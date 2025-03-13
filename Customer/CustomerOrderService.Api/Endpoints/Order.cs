using CustomerOrderService.Application.Dto;
using CustomerOrderService.Application.Order.Create;
using CustomerOrderService.Extensions;
using CustomerOrderService.Infrastructure;
using FluentValidation;
using MediatR;
using SharedKernel;

namespace CustomerOrderService.Api.Endpoints;

internal sealed class Order : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (OrderDto request,
                                      IValidator<OrderDto> validator,
                                      IConfiguration config,
                                      ISender sender,
                                      CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var command = new CreateOrderCustomerCommand(config.GetValue<string>("CNPJ")!, request);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(id => Results.Created($"/orders/{id}", new { id }), CustomResults.Problem);
        })
        .WithTags("orders");
    }
}
